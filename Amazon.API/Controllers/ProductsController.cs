using Amazon.API.Dtos;
using Amazon.API.Errors;
using Amazon.API.Helpers;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amazon.API.Controllers
{
    public class ProductsController : BaseApiController 
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get : api/Products
        [CashedAttribute(600)]
        [HttpGet] 
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(productParams);
            var products = await _unitOfWork.Reposoitory<Product>().GetAllwithSpecAsync(spec);
            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFilterForCountSpecifction(productParams);
            var Count = await _unitOfWork.Reposoitory<Product>().GetCountAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, Count, Data));
        }


        // Get : api/Products/10
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Reposoitory<Product>().GetByIdWithSpecAsync(spec);
            if (product == null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        // Get : api/Products/brands
        [CashedAttribute(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Reposoitory<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        // Get : api/Products/types
        [CashedAttribute(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Reposoitory<ProductType>().GetAllAsync();
            return Ok(types);
        }

    }
}
