using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大学生志愿者管理系统1
{
    public class cartItem
    {
        queue1DataContext db = new queue1DataContext(); 
        public ACTapply_T Add(int volunteerid, int activityid, int qty)
        {
            ACTapply_T cartItem = null;
            ActivityT activity1 = (from a in db.ActivityT where a.activity_ID == activityid select a).First();

            cartItem = new ACTapply_T();
            cartItem.Vol_ID = volunteerid;
            cartItem.Act_ID = activity1.activity_ID;
            cartItem.Act_Name = activity1.activity_Name;
            cartItem.Act_place = activity1.place;
            cartItem.Need = activity1.renshu;
            cartItem.Holder = activity1.Holder;
            cartItem.Qty = qty;
           

            int ExistAqueueCount = (from c in db.ACTapply_T where c.Vol_ID == volunteerid && c.Act_ID == activityid select c).Count();
            if (ExistAqueueCount > 0)
            {
                ACTapply_T exisAqueue = (from c in db.ACTapply_T where c.Vol_ID == volunteerid && c.Act_ID == activityid select c).First();
                exisAqueue.Qty += qty;
            }
            else
            {
                db.ACTapply_T.InsertOnSubmit(cartItem);
            }

            db.SubmitChanges();
            return cartItem;
        }

    }
}