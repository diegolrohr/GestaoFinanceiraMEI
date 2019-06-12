﻿using System.Collections.Generic;

namespace Fly01.Core.Helpers
{
    public static class NFSeTssHelper
    {
        /// <summary>
        /// Rodar o comando no console, na página do TDN da Totvs com os Ibges homologados
        /// var string = ""; $('td:first-child').each(function() { string += '"' +$(this).text() + '"' + ','}); console.log(string);
        /// Alterar também em fnFormReady dados da empresa manager
        /// </summary>
        public static List<string> IbgesCidadesHomologadasTssNFSe
        {
            get
            {
                return new List<string>()
                {
                    "1200401","2700300","2704302","2704708","2706901","1600303","1302603","2905701","2909307","2921005","2910800","2914802","2919207","2919553","2927408","2928703","2930709","2931350","2933307","2925303","2928901","2301000","2301109","2304285","2304400","2307650","2312908","3200300","3200607","3201209","3201308","3203205","3205002","3205200","3205309","5201108","5201405","5204508","5208707","5209101","5213103","5218508","2111300","5103403","5105259","5106224","5107602","5107909","5108402","5002704","5003702","5005400","5006606","3104007","3157807","3101508","3105608","3106200","3106309","3106705","3115300","3117504","3118601","3122306","3125101","3127107","3127701","3129806","3131307","3131703","3132404","3133808","3134202","3136207","3136702","3137601","3138203","3143302","3143906","3144805","3145208","3146107","3147105","3148004","3148103","3152501","3156700","3159605","3158953","3162104","3162500","3165537","3167202","3168705","3169901","3170107","3170206","3170701","3171204","1500800","1501402","1504208","2504009","2507507","4104808","4127700","4101507","4101804","4102307","4104204","4105805","4106407","4106902","4107652","4108304","4108403","4109401","4111506","4111803","4113700","4115200","4115804","4117305","4118204","4118501","4119152","4119905","4120333","4123501","4125506","2610707","2602902","2609600","2604106","2607208","2607901","2611101","2611606","2211001","3300100","3300407","3300456","3300704","3301009","3301702","3301900","3302007","3302403","3302502","3302601","3303302","3303203","3303401","3303500","3303906","3304110","3304144","3304201","3304508","3304524","3304557","3304904","3305000","3305109","3305505","3305802","3306206","3306305","2408102","4300604","4303103","4311403","4316907","4300406","4301206","4302105","4302808","4303905","4304408","4304606","4304705","4304903","4305108","4306403","4307005","4307609","4307708","4307807","4307906","4308102","4308201","4308508","4308607","4309100","4309209","4310009","4310207","4311304","4312401","4313300","4313409","4313904","4314100","4314407","4314902","4315602","4316808","4317202","4317301","4318002","4318705","4318903","4321634","4322400","4322608","1100205","1400100","4201307","4202008","4202305","4202404","4203006","4203808","4303509","4204608","4204202","4204806","4205407","4205902","4206306","4206504","4207304","4208203","4208906","4209003","4209102","4209300","4210100","4211306","4211900","4215802","4216206","4216909","4216602","4217402","4218202","4208450","3551009","3502507","3547809","3551702","3550704","3543907","3501608","3503208","3503307","3504008","3505708","3505906","3506003","3506359","3507605","3508504","3509502","3510500","3510807","3511706","3513009","3513504","3513801","3516853","3518404","3518305","3518701","3518800","3518859","3519071","3520509","3523107","3523404","3523909","3524006","3524402","3524709","3524907","3525102","3525300","3525904","3526803","3526902","3527108","3529401","3530607","3530805","3534401","3536505","3538709","3539509","3539806","3541000","3541406","3543402","3545209","3546801","3547304","3548500","3548708","3548807","3548906","3549102","3549805","3549904","3550308","3550605","3552205","3552403","3552502","3554003","3554102","3515004","3505500","3510609","3555000","2800308","2800605","2802106","2803500","1702109","1709302","1709500","1711902","1716109","1718204","1721000","3522307","3151800"
                };
            }
        }

        public static string LinkTdnCidadesHomologadas
        {
            get
            {
                return "http://tdn.totvs.com/pages/viewpage.action?pageId=4814340";
            }
        }
    }
}