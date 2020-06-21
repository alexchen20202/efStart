using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace efStart3.Services
{
    public class PagedList<T>: List<T>
    {
        public int TotalPages{get;private set;}
        public bool HasNextPage{get; private set;}
        public bool HasPrevPage{get; private set;}
        public int PageIndex{get; private set;}
        public List<T> Data{get;set;}

        public PagedList()
        {           
        }

        public async void PagingAsync(IQueryable<T> collection, int pageIndex, int pageSize)
        {
            int count = collection.Count();
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);

            if (pageIndex <= 0) { pageIndex = 1;}
            if (pageIndex > TotalPages) { pageIndex = TotalPages; }

            PageIndex = pageIndex;            
            HasNextPage =  (pageIndex < TotalPages) ? true : false;
            HasPrevPage =  (pageIndex > 1) ? true : false;
            Data = await collection.Skip(( pageIndex - 1 ) * pageSize)
            .Take(pageSize).ToListAsync();
        }

    }
}