using Project.Models;
using System.Linq;

namespace Project.ViewModels
{
    public class MallItem
    {
        public long mall_id { get; set; }
        public string mall_name { get; set; }
        public long status_id { get; set; }
        public string status_name { get; set; }
        public int number_of_pavilion { get; set; }
        public string city { get; set; }
        public decimal cost { get; set; }
        public int number_of_storeys { get; set; }
        public double value_added_factor { get; set; }
        public byte[] photo { get; set; }

        public static explicit operator Mall(MallItem mallItem)
        {
            return new Mall
            {
                mall_id = mallItem.mall_id,
                mall_name = mallItem.mall_name,
                status_id = mallItem.status_id,
                cost = mallItem.cost,
                number_of_pavilion = mallItem.number_of_pavilion,
                city = mallItem.city,
                value_added_factor = mallItem.value_added_factor,
                number_of_storeys = mallItem.number_of_storeys,
                photo = mallItem.photo,
            };
        }
        public static explicit operator MallItem(Mall mall)
        {
            return new MallItem
            {
                mall_id = mall.mall_id,
                mall_name = mall.mall_name,
                status_id = mall.status_id,
                status_name = (
                    from m in Manager.Instance.Context.Mall_statuses
                    where m.status_id == mall.status_id
                    select m.status_name
                    ).FirstOrDefault(),
                number_of_pavilion = mall.number_of_pavilion,
                city = mall.city,
                cost = mall.cost,
                number_of_storeys = mall.number_of_storeys,
                value_added_factor = mall.value_added_factor,
                photo = mall.photo
            };
        }
    }
}