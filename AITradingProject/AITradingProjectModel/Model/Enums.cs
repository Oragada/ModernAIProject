using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProjectModel.Model
{
    public enum TradeStatus
    {
        Rejected, Successful, Unable
    }

    public enum Resource { Food, Water, Dolls }

    public delegate void StatusUpdate(int cityId, StatusUpdateType type);

    public enum StatusUpdateType { HealthGained, HealthLost, PointGained }
}
