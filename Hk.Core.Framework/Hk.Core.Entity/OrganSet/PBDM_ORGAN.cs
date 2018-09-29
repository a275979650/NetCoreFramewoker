using Hk.Core.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hk.Core.Entity.OrganSet
{
    /// <summary>
    /// HKPEIS.PBDM_ORGAN
    /// </summary>
    [Table("PBDM_ORGAN")]
    public class PBDM_ORGAN:BaseModel<string>
    {
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_KEY
        /// </summary>
        [Key]
        public override string Id { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.OGRAN_SERIAL_KEY
        /// </summary>
        public Decimal OGRAN_SERIAL_KEY { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_CODE
        /// </summary>
        public String ORGAN_CODE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_NAME
        /// </summary>
        public String ORGAN_NAME { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_LEVEL
        /// </summary>
        public String ORGAN_LEVEL { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_TYPE
        /// </summary>
        public String ORGAN_TYPE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.PROVINCE
        /// </summary>
        public String PROVINCE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.CITY
        /// </summary>
        public String CITY { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.COUNTY
        /// </summary>
        public String COUNTY { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ADDRESS
        /// </summary>
        public String ADDRESS { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ZIP_CODE
        /// </summary>
        public String ZIP_CODE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.PHONE
        /// </summary>
        public String PHONE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.WEBSITE
        /// </summary>
        public String WEBSITE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.EMAIL
        /// </summary>
        public String EMAIL { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.GAIN_SIGN
        /// </summary>
        public String GAIN_SIGN { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.LOGO_PATH
        /// </summary>
        public String LOGO_PATH { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ACTIVE
        /// </summary>
        public String ACTIVE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.TRX_DATE
        /// </summary>
        public String TRX_DATE { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.TRX_USER_KEY
        /// </summary>
        public String TRX_USER_KEY { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.TRX_USER_SERIAL_KEY
        /// </summary>
        public Decimal TRX_USER_SERIAL_KEY { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_SUB_NAME
        /// </summary>
        public String ORGAN_SUB_NAME { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_EN_NAME
        /// </summary>
        public String ORGAN_EN_NAME { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.PUBLIC_TRANS
        /// </summary>
        public String PUBLIC_TRANS { get; set; }

        /// <summary>
        /// HKPEIS.PBDM_ORGAN.SUMMARY
        /// </summary>
        public String SUMMARY { get; set; }

    }
}