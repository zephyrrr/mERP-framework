using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Feng.Grid.Print
{
	/// <summary>
	/// The ReportPreviewControl class allows for a report to be previewed before it is printed or exported.
	/// </summary>
	public class ReportPreviewControl : Control
	{
    /// <summary>
    /// Initializes a new instance of the ReportPreviewControl class.
    /// </summary>
		public ReportPreviewControl()
		{
      this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
      this.SetStyle( ControlStyles.UserPaint, true );
      this.SetStyle( ControlStyles.DoubleBuffer, true );
      this.SetStyle( ControlStyles.Selectable, false );
      this.SetStyle( ControlStyles.ResizeRedraw, true );
		}

    /// <summary>
    /// Gets or sets the <see cref="System.Drawing.Printing.PrintDocument"/> which defines how a report is
    /// sent to a printer.
    /// </summary>
    /// <value>A reference to a <see cref="System.Drawing.Printing.PrintDocument"/> which defines how a report
    /// is sent to a printer.</value>
    public PrintDocument PrintDocument
    {
      get
      {
        return m_printDocument;
      }
      set
      {
        m_printDocument = value;
        this.InvalidatePreview();
      }
    }

    private void InvalidatePreview()
    {
      m_currentPreviewPage = null;
      this.InvalidateLayout();
    }

    /// <summary>
    /// Raises the <see cref="Control.SizeChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnSizeChanged( EventArgs e )
    {
      base.OnSizeChanged( e );
      this.InvalidateLayout();
    }

    /// <summary>
    /// Raises the <see cref="Control.Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint( PaintEventArgs e )
    {
      Rectangle clientRect = this.ClientRectangle;

      using( Brush brush = new SolidBrush( this.BackColor ) )
      {
        e.Graphics.FillRectangle( brush, clientRect );
      }

      if( !m_backgroundPaintedOnce )
      {
        m_backgroundPaintedOnce = true;
        this.Invalidate();
        return;
      }

      this.UpdateLayout();

      PreviewPageInfo previewPage = this.CurrentPreviewPage;

      if( previewPage != null )
      {
        Debug.Assert( !m_pageRectangle.IsEmpty, "The page rectangle should not be empty when there is a CurrentPreviewPage. It should have been calculated in UpdateLayout()." );

        Rectangle shadowRectangle = m_pageRectangle;
        shadowRectangle.Offset( ShadowThickness, ShadowThickness );

        Rectangle borderRectangle = m_pageRectangle;
        borderRectangle.Width -= 1; // Need to remove 1 pixel from the width and height for DrawRectangle to work
        borderRectangle.Height -= 1;

        // Draw the shadow
        e.Graphics.FillRectangle( Brushes.Black, shadowRectangle );

        // Draw the page
        e.Graphics.FillRectangle( Brushes.White, m_pageRectangle );

        //TODO: Ne pas dessiner si rectangle plus petit que 2x2
        // Draw the page contents
        e.Graphics.DrawImage( previewPage.Image, m_pageRectangle );

        // Draw the page border
        e.Graphics.DrawRectangle( Pens.Black, borderRectangle );
      }
      else
      {
        using( Brush brush = new SolidBrush( this.ForeColor ) )
        {
          using( StringFormat stringFormat = new StringFormat() )
          {
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString( "No Report", this.Font, brush, clientRect, stringFormat );
          }
        }
      }

      base.OnPaint( e );
    }

    private PreviewPageInfo CurrentPreviewPage
    {
      get
      {
        if( m_currentPreviewPage == null )
          this.GeneratePreview();

        return m_currentPreviewPage;
      }
    }

    private void GeneratePreview()
    {
      m_currentPreviewPage = null;

      PrintDocument document = this.PrintDocument;

      if( document == null )
        return;

      PrintController oldController = document.PrintController;

      try
      {
        Cursor.Current = Cursors.WaitCursor;

        PreviewPrintController controller = new PreviewPrintController();

        document.PrintController = controller;
        document.PrintPage += new PrintPageEventHandler( document_PrintPage );
        document.Print();
        document.PrintPage -= new PrintPageEventHandler( document_PrintPage );

        PreviewPageInfo[] pages = controller.GetPreviewPageInfo();

        m_currentPreviewPage = pages[ 0 ];
      }
      finally
      {
        document.PrintController = oldController;
        Cursor.Current = Cursors.Default;
      }
    }

    private void document_PrintPage(object sender, PrintPageEventArgs e)
    {
      // Setting e.Cancel to true will limit the preview to 1 page.
      e.Cancel = true;
    }

    private void InvalidateLayout()
    {
      if( m_layoutValid )
      {
        m_layoutValid = false;
        this.Invalidate();
      }
    }

    private void UpdateLayout()
    {
      if( m_layoutValid )
        return;

      this.ResetLayout();

      PreviewPageInfo previewPage = this.CurrentPreviewPage;

      if( previewPage != null )
      {
        Rectangle clientRectangle = this.ClientRectangle;

        clientRectangle.Width -= ShadowThickness;
        clientRectangle.Height -= ShadowThickness;

        float scale = Math.Min( 
          ( float )clientRectangle.Width / previewPage.PhysicalSize.Width,
          ( float )clientRectangle.Height / previewPage.PhysicalSize.Height );
      
        int pageWidth = ( int )( previewPage.PhysicalSize.Width * scale );
        int pageHeight = ( int )( previewPage.PhysicalSize.Height * scale );

        m_pageRectangle = new Rectangle(
          ( int )( ( clientRectangle.Width - pageWidth ) / 2 ),
          ( int )( ( clientRectangle.Height - pageHeight ) / 2 ),
          pageWidth,
          pageHeight );
      }

      m_layoutValid = true;
    }

    private void ResetLayout()
    {
      m_pageRectangle = Rectangle.Empty;
    }

    private PrintDocument m_printDocument; // = null
    private bool m_backgroundPaintedOnce; // = false
    private bool m_layoutValid; // = false
    private PreviewPageInfo m_currentPreviewPage; // = null
    private Rectangle m_pageRectangle; // = Rectangle.Empty

    private const int ShadowThickness = 2;
	}
}
