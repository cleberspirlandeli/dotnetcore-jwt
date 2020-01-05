using App.Data;
using App.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public class ProdutoService
    {
        private readonly ApplicationServiceContext _context;

        public ProdutoService(ApplicationServiceContext context)
        {
            _context = context;
        }

        //public async Task<IQueryable<ProdutoDto>> GetAllProduto()
        //{
        //    var result = await _context.Produtos.FindAsync();
        //    return result;
        //}

    }
}
