using System.Collections.Generic;

namespace DepartmentalTraversal
{
    public class Rule
    {
        private readonly int _goodStamp;
        private readonly int _badStamp;
        private readonly int _nextDepartment;
        
        public Rule(int goodStamp, int badStamp, int nextDepartment)
        {
            _goodStamp = goodStamp;
            _badStamp = badStamp;
            _nextDepartment = nextDepartment;
        }

        public int Run(HashSet<int> list)
        {
            if (list.Contains(_badStamp)) 
                list.Remove(_badStamp);
            
            list.Add(_goodStamp);
            
            return _nextDepartment;
        }
    }
}