using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.Builders
{
    public class EntityBuilder
    {
        private BusinessUnit _entity;
        private BusinessUnitType _type;
        private string _code;
        public string Code => _code;
        private string _description;

        public BusinessUnit Build()
        {
            if (_entity == null)
            {
                _entity = new BusinessUnit()
                {
                    BusinessUnitType = _type,
                    Code = _code,
                    Name = _description
                };
            }
            return _entity;
        }

        public EntityBuilder AsType(BusinessUnitType type)
        {
            _type = type;
            return this;
        }

        public EntityBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public EntityBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public EntityBuilder WithSimpleEntry(BusinessUnitType type, string code, string description)
        {
            AsType(type);
            WithCode(code);
            WithDescription(description);
            return this;
        }
    }
}
