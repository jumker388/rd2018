using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Web365Domain;
using Web365Models;

namespace Web365.Models
{
    public class NewsHomeModels
    {
        public ListArticleModel LeftModel;
        public ListArticleModel RightModel;
    }
}
