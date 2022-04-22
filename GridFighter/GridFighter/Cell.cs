using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridFighter
{
    class Cell
    {
        private Boolean Selected, Matched, Visited, Untouchable;
        private int CellID;

        public void setCellID(int ID)
        {
            CellID = ID;
        }
        public int getCellID()
        {
            return CellID;
        }
        public void setSelected(Boolean Select)
        {
            Selected = Select;
        }
        public Boolean getSelected()
        {
            return Selected;
        }
        public void setMatched(Boolean Match)
        {
            Matched = Match;
        }
        public Boolean getMatched()
        {
            return Matched;
        }
        public void setVisited(Boolean Visit)
        {
            Visited = Visit;
        }
        public Boolean getVisited()
        {
            return Visited;
        }
        public void setUntouchable(Boolean neTouchePas)
        {
            Untouchable = neTouchePas;
        }
        public Boolean getUntouchable()
        {
            return Untouchable;

        }

    }
}
