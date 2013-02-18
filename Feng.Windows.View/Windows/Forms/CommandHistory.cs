using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    public class CommandHistory
    {
        int MAX_SIZE = 50;

        List<string> _cmdHistory;
        int _curIdx;

        internal CommandHistory()
        {
            _cmdHistory = new List<string>();
        }

        internal List<string> GetAll()
        {
            return _cmdHistory;
        }

        internal void Add(string command)
        {
            if (_cmdHistory.Count >= MAX_SIZE)
            {
                _cmdHistory.RemoveAt(0);
            }
            _cmdHistory.Add(command);
            _curIdx = _cmdHistory.Count;
        }

        internal string GetPrevious()
        {
            _curIdx = (_curIdx <= 0) ? 0 : _curIdx - 1;
            return _cmdHistory[_curIdx];
        }

        internal string GetNext()
        {
            _curIdx = (_curIdx >= _cmdHistory.Count - 1) ? _cmdHistory.Count - 1 : _curIdx + 1;
            return _cmdHistory[_curIdx];
        }
    }
}
