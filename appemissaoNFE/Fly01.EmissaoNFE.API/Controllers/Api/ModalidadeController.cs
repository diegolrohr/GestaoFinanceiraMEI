﻿using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("modalidade")]
    public class ModalidadeController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(ParametroVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.ModalidadeBL.ValidaModel(entity);

                try
                {
                    var homolog = new SPEDCFGNFE.SPEDCFGNFE().CFGMODALIDADEEX(AppDefault.Token, entity.Homologacao, entity.TipoModalidade);

                    var prod = new SPEDCFGNFEProd.SPEDCFGNFE().CFGMODALIDADEEX(AppDefault.Token, entity.Producao, entity.TipoModalidade);

                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    if (unitOfWork.EntidadeBL.TSSException(ex))
                    {
                        unitOfWork.EntidadeBL.EmissaoNFeException(ex, entity);
                    }

                    return InternalServerError(ex);
                }
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}