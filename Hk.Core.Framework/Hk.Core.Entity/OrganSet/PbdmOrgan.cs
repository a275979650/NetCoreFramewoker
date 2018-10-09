using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hk.Core.Data.Models;

namespace Hk.Core.Entity.OrganSet
{
    /// <summary>
    /// HKPEIS.PBDM_ORGAN
    /// </summary>
    [Table("PBDM_ORGAN"), Serializable]
    public class PbdmOrgan:BaseModel<string>
    {
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_KEY
        /// </summary>
        [Key]
        [Column("Id")]
        public override String Id { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.OGRAN_SERIAL_KEY
        /// </summary>
        
        [Column("OGRAN_SERIAL_KEY")]
        public Decimal OgranSerialKey { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_CODE
        /// </summary>
        
        [Column("ORGAN_CODE")]
        public String OrganCode { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_NAME
        /// </summary>
        
        [Column("ORGAN_NAME")]
        public String OrganName { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_LEVEL
        /// </summary>
        
        [Column("ORGAN_LEVEL")]
        public String OrganLevel { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_TYPE
        /// </summary>
        
        [Column("ORGAN_TYPE")]
        public String OrganType { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.PROVINCE
        /// </summary>
        
        [Column("PROVINCE")]
        public String Province { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.CITY
        /// </summary>
        
        [Column("CITY")]
        public String City { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.COUNTY
        /// </summary>
        
        [Column("COUNTY")]
        public String County { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ADDRESS
        /// </summary>
        
        [Column("ADDRESS")]
        public String Address { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ZIP_CODE
        /// </summary>
        
        [Column("ZIP_CODE")]
        public String ZipCode { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.PHONE
        /// </summary>
        
        [Column("PHONE")]
        public String Phone { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.WEBSITE
        /// </summary>
        
        [Column("WEBSITE")]
        public String Website { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.EMAIL
        /// </summary>
        
        [Column("EMAIL")]
        public String Email { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.GAIN_SIGN
        /// </summary>
        
        [Column("GAIN_SIGN")]
        public String GainSign { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.LOGO_PATH
        /// </summary>
        
        [Column("LOGO_PATH")]
        public String LogoPath { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ACTIVE
        /// </summary>
        
        [Column("ACTIVE")]
        public String Active { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.TRX_DATE
        /// </summary>
        
        [Column("TRX_DATE")]
        public String TrxDate { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.TRX_USER_KEY
        /// </summary>
        
        [Column("TRX_USER_KEY")]
        public String TrxUserKey { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.TRX_USER_SERIAL_KEY
        /// </summary>
        
        [Column("TRX_USER_SERIAL_KEY")]
        public Decimal TrxUserSerialKey { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_SUB_NAME
        /// </summary>
        
        [Column("ORGAN_SUB_NAME")]
        public String OrganSubName { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.ORGAN_EN_NAME
        /// </summary>
        
        [Column("ORGAN_EN_NAME")]
        public String OrganEnName { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.PUBLIC_TRANS
        /// </summary>
        
        [Column("PUBLIC_TRANS")]
        public String PublicTrans { get; set; }
        
        /// <summary>
        /// HKPEIS.PBDM_ORGAN.SUMMARY
        /// </summary>
        
        [Column("SUMMARY")]
        public String Summary { get; set; }
        
    }
}