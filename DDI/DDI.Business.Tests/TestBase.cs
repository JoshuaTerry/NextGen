using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests
{    
    public abstract class TestBase
    {
        /// <summary>
        /// Perform an action and assert that no exception is thrown.
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="message">Message provided to Assert</param>
        protected void AssertNoException(Action action, string message = "")
        {
            try
            {
                action.Invoke();
            }
            catch
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Perform an action and assert that a specified exception type is thrown.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <param name="action">Action to perform</param>
        /// <param name="message">Message provided to Assert</param>
        protected void AssertThrowsException<T>(Action action, string message = "") where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T)
            {
                return;
            }
            Assert.Fail(message);
        }

        /// <summary>
        /// Perform an action and assert that a specified exception type is thrown and the exception message contains a specified string.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <param name="action">Action to perform</param>
        /// <param name="contains_text">Text expected in exception message</param>
        /// <param name="message">Message provided to Assert</param>
        protected void AssertThrowsExceptionMessageContains<T>(Action action, string contains_text, string message = "") where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T ex) when (ex.Message.Contains(contains_text))
            {
                    return;
            }
            Assert.Fail(message);
        }


    }
}
