using System.Collections.Generic;

namespace DepartmentalTraversal
{
    public class SpecialDepartment : Department
    {
        private readonly Rule _rule;
        private readonly int _condition;
        
        public SpecialDepartment(int number, Rule firstRule, Rule secondRule, int condition) : 
            base(number, firstRule)
        {
            _rule = secondRule;
            _condition = condition;
        }

        public override int GetNext(HashSet<int> list)
        {
            if (list.Contains(_condition))
                return base.GetNext(list);
            return _rule.Run(list);
        }
    }
}