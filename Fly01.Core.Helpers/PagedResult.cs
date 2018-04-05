using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Helpers
{
    public class PagedResult<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; private set; }

        [JsonProperty("paging")]
        public PagingInfo Paging { get; private set; }

        public PagedResult(IEnumerable<T> items, int pageNo, int pageSize, long totalRecordCount)
        {
            Data = items == null ? new List<T>() : new List<T>(items);

            Paging = new PagingInfo
            {
                PageNo = pageNo,
                PageSize = pageSize,
                TotalRecordCount = totalRecordCount,
                PageCount = totalRecordCount > 0
                    ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                    : 0
            };
        }
    }

    public class PagingInfo
    {
        [JsonProperty("pageNo")]
        public int PageNo { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("totalRecordCount")]
        public long TotalRecordCount { get; set; }
    }
}