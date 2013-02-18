using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;


namespace CredentialsManagerClient
{
   public class ListViewEx : ListView
   {
      public override string Text
      {
         get
         {
            return CurrentListViewItem;
         }         
         set
         {
            Debug.Assert(false);
            base.Text = value;
         }
      }

      public string[] ToArray()
      {
         List<string> list = new List<string>(Items.Count);
         foreach(object obj in Items)
         {
            list.Add((obj as ListViewItem).Text);
         }
         list.Sort();
         return list.ToArray();

      }
      public bool HasSelection
      {
         get
         {
            if(SelectedItems.Count > 0)
            {
               return true;
            }
            else
            {
               return false;
            }
         }
      }
      public ListViewItem FindItem(string text)
      {
         foreach(ListViewItem item in Items)
         {
            if(item.Text == text)
            {
               return item;
            }
         }
         return null;
      }
      public ListViewItem FindItem(int index)
      {
         Debug.Assert(Items.Count >= index && 0 <= index);
         return Items[index];
      }
      public int FindIndex(string text)
      {
         foreach(ListViewItem item in Items)
         {
            if(item.Text == text)
            {
               return item.Index;
            }
         }
         return -1;
      }
      public void RemoveItem(string text)
      {
         Debug.Assert(Items.Count > 0);
         ListViewItem item = FindItem(text);
         int index = FindIndex(text);
         Items.Remove(item);

         if(Items.Count == 0)
         {
            return;
         }
         if(Items.Count == 1 || index == 0)
         {
            Items[0].Selected = true;
         }
         if(Items.Count == index)
         {
            item = FindItem(index-1);
            item.Selected = true;
         }
         else
         {
            item = FindItem(index);
            item.Selected = true;
         }
         Select();
      }
      public void ClearItems()
      {
         Items.Clear();
      }
      public ListViewItem AddItem(string text,bool selected)
      {
         Debug.Assert(text != String.Empty);
         ListViewItem item = new ListViewItem(text,0);
         Items.Add(item);
         if(selected)
         {
            item.Selected = true;
            Select();
         }
         return item;
      }
      public void AddItems(string[] items,bool selectFirst)
      {
         ListViewItem first = null;
         Debug.Assert(items != null);
         if(items.Length == 0)
         {
            return;
         }
         ListViewItem item = null;
         foreach(string text in items)
         {
            item = new ListViewItem(text,0);
            if(first == null)
            {
               first = item;
            }
            Items.Add(item);
         }
         if(selectFirst)
         {
            first.Selected = true;
         }
         else
         {
            item.Selected = true;
         }
         Select();
      }
      public string CurrentListViewItem
      {
         get
         {
            Debug.Assert(MultiSelect == false);
            if(SelectedItems.Count == 1)
            {
               IEnumerator enumerator = SelectedItems.GetEnumerator();
               enumerator.MoveNext();
               ListViewItem item = enumerator.Current as ListViewItem;
               return item.Text;
            }
            if(SelectedItems.Count == 0)
            {
               return String.Empty;
            }
            else
            {
               Debug.Assert(false);
               return null;
            }
         }
      }
   }
}
