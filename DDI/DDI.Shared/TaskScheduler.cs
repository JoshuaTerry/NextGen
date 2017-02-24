using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDI.Shared
{
    /* NOTE:  This class implements a simple task scheduler that performs actions on a background thread.  
     * It will ultimately need to be replaced by a more robust facility that uses Windows Message Queue 
     * to ensure that actions are performed transactionally with the ability to remove duplicate actions.
     */

    /// <summary>
    /// A simple task scheduler than can be used to schedule actions to be executed on a background thread.
    /// </summary>
    public static class TaskScheduler
    {
        #region Private Fields

        private const int POLLING_INTERVAL = 500; // Milliseconds

        private static object _lockObject;
        private static List<ScheduledTask> _taskList;
        private static Task _taskRunner = null;
        private static bool _cancel = false;

        #endregion

        #region Constructors

        static TaskScheduler()
        {
            _lockObject = new object();
            _taskList = new List<ScheduledTask>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Schedule an action, optionally waiting a number of seconds.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <param name="waitSeconds">Minimum number of seconds to wait before executing the action.</param>
        public static void ScheduleTask(Action action, int waitSeconds = 0)
        {
            Initialize();

            lock (_lockObject)
            {
                if (!_cancel)
                {
                    _taskList.Add(new ScheduledTask(action, DateTime.Now.AddSeconds(waitSeconds)));
                }
            }
        }

        /// <summary>
        /// Cancel any scheduled tasks.  The task runner terminates.
        /// </summary>
        public static void CancelScheduledTasks()
        {
            lock(_lockObject)
            {
                _cancel = true;
                _taskList.Clear();
            }
        }

        #endregion

        #region Private Methods

        private static void Initialize()
        {
            lock (_lockObject)
            {
                if (_taskRunner == null)
                {
                    RunTaskLoop();
                }
            }
        }

        private static void RunTaskLoop()
        {
            _taskRunner = Task.Run(() =>
            {
                while (!_cancel)
                {
                    Thread.Sleep(POLLING_INTERVAL);

                    if (_cancel)
                    {
                        break;
                    }

                    List<ScheduledTask> readyTasks;

                    lock (_lockObject)
                    {
                        // Build a list of tasks ready to run and remove them from the queued tasks.
                        DateTime now = DateTime.Now;
                        readyTasks = _taskList.Where(p => p.StartTime < now).ToList();

                        foreach (var entry in readyTasks)
                        {
                            _taskList.Remove(entry);
                        }
                    }

                    // Run each of the tasks in readyTasks
                    foreach (var entry in readyTasks)
                    {
                        if (_cancel)
                        {
                            break;
                        }
                        entry.Action.Invoke();
                    }
                }
            });
        }
        #endregion

        #region Internal Classes

        /// <summary>
        /// Internal class representing an action and a start DateTime.
        /// </summary>
        private class ScheduledTask
        {
            public DateTime StartTime { get; set; }
            public Action Action { get; set; }

            public ScheduledTask(Action action, DateTime startTime)
            {
                Action = action;
                StartTime = startTime;
            }
        }

        #endregion

    }
}
