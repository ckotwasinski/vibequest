using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace VibeQuest.Service.Impl
{
    public class BaseService
    {
        protected readonly IMapper _mapper;
        public BaseService(IMapper Mapper)
        {
            _mapper = Mapper;
        }
    }
}
