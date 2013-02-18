using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

public partial class ComboBoxEx : ComboBox
{
   ImageList m_ImageList;
   public ImageList ImageList
   {
      get 
      {
         return m_ImageList;
      }
      set 
      {
         m_ImageList = value;
      }
   }

   public void SetImage(Image image)
   {
      DrawMode = DrawMode.OwnerDrawFixed;
      ImageList = new ImageList();
      ImageList.Images.Add(image);
   }
   public void SetImage(Icon icon)
   {
      DrawMode = DrawMode.OwnerDrawFixed;
      ImageList = new ImageList();
      ImageList.Images.Add(icon);
   }

   public void RefreshComboBox(string caption,string[] items)
   {
      Items.Clear();

      Text = caption ?? String.Empty;

      if(items == null)
      {
         Enabled = false;
         return;
      }
      if(items.Length == 0)
      {
         Enabled = false;
         return;
      }
      Enabled = true;
      AddItems(items);
   } 
   public void AddItem(string item)
   {
      Items.Add(new ComboBoxExItem(item,0));
   }
   public void AddItems(string[] items)
   {
      Array.ForEach(items,AddItem);
   }
   protected override void OnDrawItem(DrawItemEventArgs itemEventArgs)
   {
      itemEventArgs.DrawBackground();
      itemEventArgs.DrawFocusRectangle();

      ComboBoxExItem item;
      System.Drawing.Size imageSize = m_ImageList.ImageSize;
      System.Drawing.Rectangle bounds = itemEventArgs.Bounds;

      try
      {
         item = (ComboBoxExItem)Items[itemEventArgs.Index];

         if (item.ImageIndex != -1)
         {
            m_ImageList.Draw(itemEventArgs.Graphics, bounds.Left, bounds.Top,item.ImageIndex);

            itemEventArgs.Graphics.DrawString(item.Text, itemEventArgs.Font,new SolidBrush(itemEventArgs.ForeColor), bounds.Left+imageSize.Width, bounds.Top);
         }
         else
         {
            itemEventArgs.Graphics.DrawString(item.Text, itemEventArgs.Font, new SolidBrush(itemEventArgs.ForeColor), bounds.Left, bounds.Top);
         }
      }
      catch
      {
         if (itemEventArgs.Index != -1)
         {
            itemEventArgs.Graphics.DrawString(Items[itemEventArgs.Index].ToString(), itemEventArgs.Font, new SolidBrush(itemEventArgs.ForeColor), bounds.Left, bounds.Top);
         }
         else
         {
            itemEventArgs.Graphics.DrawString(Text, itemEventArgs.Font, new SolidBrush(itemEventArgs.ForeColor), bounds.Left, bounds.Top);
         }
      }
      base.OnDrawItem(itemEventArgs);
   }
}

class ComboBoxExItem
{
   string m_Text;
   int m_ImageIndex;
   public string Text
   {
      get 
      {
         return m_Text;
      }
      set 
      {
         m_Text = value;
      }
   }

   public int ImageIndex
   {
      get 
      {
         return m_ImageIndex;
      }
      set 
      {
         m_ImageIndex = value;
      }
   }

   public ComboBoxExItem() : this(String.Empty) 
   {}

   public ComboBoxExItem(string text) : this(text,-1) 
   {}

   public ComboBoxExItem(string text, int imageIndex)
   {
      m_Text = text;
      m_ImageIndex = imageIndex;
   }

   public override string ToString()
   {
      return m_Text;
   }
}



