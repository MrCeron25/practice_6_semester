namespace Project.ViewModels
{
    public class PavilionItem
    {
        public long mall_id { get; set; } // id ТЦ
        public long mall_status_id { get; set; } // статус ТЦ
        public string mall_status_name { get; set; } // статус ТЦ
        public string mall_name { get; set; } // название ТЦ
        public int floor { get; set; } // номера этажа
        public string pavilion_number { get; set; } // номера павильона
        public decimal square { get; set; } // площадь
        public long pavilion_status_id { get; set; } // статус павильона
        public string pavilion_status_name { get; set; } // статус павильона
        public decimal cost_per_square_meter { get; set; } // cтоимость кв. м.
        public double value_added_factor { get; set; } // коэффициент добавочной стоимости павильона
    }
}
