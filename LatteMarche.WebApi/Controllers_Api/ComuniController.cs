using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.WebApi.Filters;
using LatteMarche.Application.Common.Dtos;
using WeCode.MVC.Attributes;

namespace LatteMarche.WebApi.Controllers_Api
{
    [ApiCustomAuthorize]
    [ApiActionFilter]
    [ApiExceptionFilter]
    public class ComuniController : ApiController
    {

        #region Fields

        private IComuniService comuniService;

        #endregion

        #region Constructors

        public ComuniController(IComuniService comuniService)
        {
            this.comuniService = comuniService;
        }

        #endregion

        #region Methods


        [ViewItem(nameof(Dropdown), "Comuni", "Dropdown")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Dropdown(string siglaProvincia)
        {

            return Ok(this.comuniService.DropDown(siglaProvincia));

        }

        [ViewItem(nameof(Province), "Comuni", "Province")]
        [HttpGet]
        [ETag]
        public IHttpActionResult Province()
        {

            var province = ((IComuniService)this.comuniService).GetProvince();
            DropDownDto dropDown = new DropDownDto();
            foreach (var prov in province)
            {
                dropDown.Items.Add(new DropDownItem() { Value = prov, Text = prov });
            }
            return Ok(dropDown);

        }

        #endregion

    }
}
