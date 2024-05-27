﻿using UI.ViewModels.Shared;

namespace UI.ViewModels.Transactions
{
    public class IndexVM
    {

        public List<DetailsVM> Items { get; set; }
        public FilterVM Filter { get; set; }
        public PagerVM Pager { get; set; }
        public SortVM OrderBy { get; set; }
    }
}
