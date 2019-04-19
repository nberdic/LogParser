﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public interface IFilter
    {
        ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search);
    }
}
