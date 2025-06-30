﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Repositories.Abstractions;

public interface IRepository<T> where T : class
{
    Task<T?> GetById(int id);

    Task<IEnumerable<T>> GetAll();
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}