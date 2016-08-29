/********************************************************************************
 *    Project   : Awesomium.NET (PoGo.NecroBot.GUI)
 *    File      : MainWindow.xaml.cs
 *    Version   : 1.7.4.1
 *    Date      : 03/21/2014
 *    Author    : Perikles C. Stephanidis (perikles@awesomium.com)
 *    Copyright : ©2014 Awesomium Technologies LLC
 *-------------------------------------------------------------------------------
 *
 *    Notes     :
 *
 *    Starter sample demonstration of the basic code in a Window hosting
 *    a WPF WebControl.
 *    
 *    
 ********************************************************************************/

#region Using
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Awesomium.Core;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using Awesomium.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MahApps.Metro.Controls;
#endregion

namespace PoGo.NecroBot.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Fields
        private const string CHANGE_IMG_SRC = @"(function() { " +
            "var image = document.getElementById('awe-logo-silhouette'); " +
            "if (image == null) { return; } " +
            "image.addEventListener('load', function() { myObject.onImageLoaded('awe-logo-silhouette', image.src) }, false ); " +
            "image.src='http://upload.wikimedia.org/wikipedia/commons/1/1e/Lascaux_painting.jpg'; })()";
        #endregion


        #region Ctors
        public MainWindow()
        {
            InitializeComponent();

            // Always handle ShowCreatedWebView. This is fired for
            // links and forms with |target="_blank"| or for JavaScript
            // 'window.open' calls.
            webControl.ShowCreatedWebView += webControl_ShowCreatedWebView;
            // We demonstrate interaction with the page. We handle these events
            // and execute the examples, only on the initial main window.
            webControl.NativeViewInitialized += OnNativeViewInitialized;
            webControl.DocumentReady += OnDocumentReady;
            webControl.ConsoleMessage += OnConsoleMessage;
            // Start with the specified Home URL.
            this.Source = WebCore.Configuration.HomeURL;
        }

        public MainWindow( IntPtr nativeView )
        {
            InitializeComponent();

            // Always handle ShowCreatedWebView. This is fired for
            // links and forms with |target="_blank"| or for JavaScript
            // 'window.open' calls.
            webControl.ShowCreatedWebView += webControl_ShowCreatedWebView;
            webControl.DocumentReady += OnDocumentReady;
            webControl.ConsoleMessage += OnConsoleMessage;
            // For popups, you usually want to handle WindowClose,
            // fired when the page calls 'window.close'.
            webControl.WindowClose += webControl_WindowClose;
            // Tell the WebControl that is should wrap a created child view.
            this.NativeView = nativeView;
            // This window will host a WebControl that is the result of 
            // JavaScript 'window.open'. Hide the address and status bar.
            this.IsRegularWindow = false;
        }

        public MainWindow( Uri url )
        {
            InitializeComponent();

            // Always handle ShowCreatedWebView. This is fired for
            // links and forms with |target="_blank"| or for JavaScript
            // 'window.open' calls.
            webControl.ShowCreatedWebView += webControl_ShowCreatedWebView;
            webControl.DocumentReady += OnDocumentReady;
            webControl.ConsoleMessage += OnConsoleMessage;
            // For popups, you usually want to handle WindowClose,
            // fired when the page calls 'window.close'.
            webControl.WindowClose += webControl_WindowClose;
            // Tell the WebControl to load a specified target URL.
            this.Source = url;
        }
        #endregion


        #region Overrides
        protected override void OnClosed( EventArgs e )
        {
            base.OnClosed( e );

            // Destroy the WebControl and its underlying view.
            webControl.Dispose();
        }
        #endregion

        #region Methods
        // We demonstrate getting a screenshot of the view, in WPF.
        private void GetScreenshot( string fileName )
        {
            WebViewPresenter surface = webControl.Surface as WebViewPresenter;

            if ( surface == null )
                return;

            BitmapSource bitmap = surface.Image as BitmapSource;

            if ( bitmap == null )
                return;

            // For the sample, we use a PNG encoder. WPF provides
            // other too, such as a JpegBitmapEncoder.
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add( BitmapFrame.Create( bitmap ) );

            // Open/Create the file to save the image to.
            using ( FileStream fs = File.Open( fileName, FileMode.OpenOrCreate ) )
                encoder.Save( fs );
        }

        // We demonstrate thread-safe execution against the view.
        private void PrintPageHTML()
        {
            // Code potentially executed in another (non-UI) thread.
            // This code is valid for any web-view component. For WPF only,
            // you can also use webControl.Dispatcher.Invoke.
            ISynchronizeInvoke sync = (ISynchronizeInvoke)webControl;
            string html = sync.InvokeRequired ?
                sync.Invoke( (Func<String>)ExecuteJavascriptOnView, null ) as String :
                ExecuteJavascriptOnView();

            if ( !String.IsNullOrEmpty( html ) )
            {
                // Managed IWebView instances already provide an HTML property
                // that is updated when the DOM is loaded. At this point,
                // these two should be equal.
                // Debug.Assert( html == webControl.HTML );
                // Print it.
                Debug.Print( html );
            }
        }

        // This method will be called on the WebControl's thread (the UI thread).
        private string ExecuteJavascriptOnView()
        {
            // This method is called from within DocumentReady so it already
            // executes in an asynchronous Javascript Execution Context (JEC).
            var global = Global.Current;

            if ( !global )
                return String.Empty;

            // We demonstrate the use of dynamic.
            var document = global.document;

            if ( !document )
                return String.Empty;

            // JSObject supports the DLR. You can dynamically call methods,
            // access arrays or lists and get or set properties.
            return document.getElementsByTagName( "html" )[ 0 ].outerHTML;

            // Any binding errors or exceptions that occur in a Javascript Execution 
            // Context (JEC) are propagated to the JavaScript console.
        }
        #endregion

        #region Properties
        // This will be set to the target URL, when this window does not
        // host a created child view. The WebControl, is bound to this property.
        public Uri Source
        {
            get { return (Uri)GetValue( SourceProperty ); }
            set { SetValue( SourceProperty, value ); }
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register( "Source",
            typeof( Uri ), typeof( MainWindow ),
            new FrameworkPropertyMetadata( null ) );


        // This will be set to the created child view that the WebControl will wrap,
        // when ShowCreatedWebView is the result of 'window.open'. The WebControl, 
        // is bound to this property.
        public IntPtr NativeView
        {
            get { return (IntPtr)GetValue( NativeViewProperty ); }
            private set { this.SetValue( MainWindow.NativeViewPropertyKey, value ); }
        }

        private static readonly DependencyPropertyKey NativeViewPropertyKey =
            DependencyProperty.RegisterReadOnly( "NativeView",
            typeof( IntPtr ), typeof( MainWindow ),
            new FrameworkPropertyMetadata( IntPtr.Zero ) );

        /// <summary>
        /// Identifies the <see cref="NativeView"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NativeViewProperty =
            NativeViewPropertyKey.DependencyProperty;

        // The visibility of the address bar and status bar, depends
        // on the value of this property. We set this to false when
        // the window wraps a WebControl that is the result of JavaScript
        // 'window.open'.
        public bool IsRegularWindow
        {
            get { return (bool)GetValue( IsRegularWindowProperty ); }
            private set { this.SetValue( MainWindow.IsRegularWindowPropertyKey, value ); }
        }

        private static readonly DependencyPropertyKey IsRegularWindowPropertyKey =
            DependencyProperty.RegisterReadOnly( "IsRegularWindow",
            typeof( bool ), typeof( MainWindow ),
            new FrameworkPropertyMetadata( true ) );

        /// <summary>
        /// Identifies the <see cref="IsRegularWindow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRegularWindowProperty =
            IsRegularWindowPropertyKey.DependencyProperty;
        #endregion

        #region Event Handlers
        private void OnNativeViewInitialized( object sender, WebViewEventArgs e )
        {
            // The native view is created. You can create global JavaScript objects
            // at this point. These objects persist throughout the lifetime of the view
            // and are available to all pages loaded by this view.
            using ( JSObject myObject = webControl.CreateGlobalJavascriptObject( "myObject" ) )
                // Add a custom method to the global object and bind to it.
                myObject.BindAsync( onImageLoaded );
        }

        private void OnDocumentReady( object sender, DocumentReadyEventArgs e )
        {
            // When ReadyState is Ready, you can execute JavaScript against
            // the DOM but all resources are not yet loaded. Wait for Loaded.
            if ( e.ReadyState == DocumentReadyState.Ready )
                return;

            // Get a screenshot of the view.
            GetScreenshot( "wpf_screenshot_before.png" );

            // Print the page's HTML source.
            PrintPageHTML();

            // Asynchronously execute some script on the page,
            // that will change the source of an image and wait
            // for the image load to complete.
            webControl.ExecuteJavascript( CHANGE_IMG_SRC );
        }

        private void onImageLoaded( object sender, JavascriptMethodEventArgs e )
        {
            Debug.Print( String.Format( "IMG with id: '{0}' completed loading: {1}", e.Arguments[ 0 ], e.Arguments[ 1 ] ) );
            // Get another screenshot.
            GetScreenshot( "wpf_screenshot_after.png" );
        }

        // Any JavaScript errors or JavaScript console.log calls,
        // will call this method.
        private void OnConsoleMessage( object sender, ConsoleMessageEventArgs e )
        {
            Debug.Print( "[Line: " + e.LineNumber + "] " + e.Message );
        }

        private void webControl_ShowCreatedWebView( object sender, ShowCreatedWebViewEventArgs e )
        {
            if ( webControl == null )
                return;

            if ( !webControl.IsLive )
                return;

            // An instance of our application's web window, that will
            // host the new view instance, either we wrap the created child view,
            // or we let the WebControl create a new underlying web-view.
            MainWindow newWindow;

            // Treat popups differently. If IsPopup is true, the event is always
            // the result of 'window.open' (IsWindowOpen is also true, so no need to check it).
            // Our application does not recognize user defined, non-standard specs. 
            // Therefore child views opened with non-standard specs, will not be presented as 
            // popups but as regular new windows (still wrapping the child view however -- see below).
            if ( e.IsPopup && !e.IsUserSpecsOnly )
            {
                // JSWindowOpenSpecs.InitialPosition indicates screen coordinates.
                Int32Rect screenRect = e.Specs.InitialPosition.GetInt32Rect();

                // Set the created native view as the underlying view of the
                // WebControl. This will maintain the relationship between
                // the parent view and the child, usually required when the new view
                // is the result of 'window.open' (JS can access the parent window through
                // 'window.opener'; the parent window can manipulate the child through the 'window'
                // object returned from the 'window.open' call).
                newWindow = new MainWindow( e.NewViewInstance );
                // Do not show in the taskbar.
                newWindow.ShowInTaskbar = false;
                newWindow.Topmost = true;
                // Set a border-style to indicate a popup.
                newWindow.WindowStyle = WindowStyle.ToolWindow;
                // Set resizing mode depending on the indicated specs.
                newWindow.ResizeMode = e.Specs.Resizable ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;

                // If the caller has not indicated a valid size for the new popup window,
                // let it be opened with the default size specified at design time.
                if ( ( screenRect.Width > 0 ) && ( screenRect.Height > 0 ) )
                {
                    // The indicated size, is client size.
                    double horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
                    double verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
                    double captionHeight = SystemParameters.CaptionHeight;

                    // Set the indicated size.
                    newWindow.Width = screenRect.Width + ( verticalBorderWidth * 2 );
                    newWindow.Height = screenRect.Height + captionHeight + ( horizontalBorderHeight * 2 );
                }

                // Show the window.
                newWindow.Show();

                // If the caller has not indicated a valid position for the new popup window,
                // let it be opened in the default position specified at design time.
                if ( ( screenRect.Y >= 0 ) && ( screenRect.X >= 0 ) )
                {
                    // Move it to the indicated coordinates.
                    newWindow.Top = screenRect.Y;
                    newWindow.Left = screenRect.X;
                }
            }
            else if ( e.IsWindowOpen || e.IsPost )
            {
                // No specs or only non-standard specs were specified, but the event is still 
                // the result of 'window.open' or of an HTML form with target="_blank" and method="post".
                // We will open a normal window but we will still wrap the new native child view, 
                // maintaining its relationship with the parent window.
                newWindow = new MainWindow( e.NewViewInstance );
                // Show the window.
                newWindow.Show();
            }
            else
            {
                // The event is not the result of 'window.open' or of an HTML form with target="_blank" 
                // and method="post"., therefore it's most probably the result of a link with target='_blank'. 
                // We will not be wrapping the created view; we let the WebControl hosted in ChildWindow 
                // create its own underlying view. Setting Cancel to true tells the core to destroy the 
                // created child view.
                //
                // Why don't we always wrap the native view passed to ShowCreatedWebView?
                //
                // - In order to maintain the relationship with their parent view,
                // child views execute and render under the same process (awesomium_process)
                // as their parent view. If for any reason this child process crashes,
                // all views related to it will be affected. When maintaining a parent-child 
                // relationship is not important, we prefer taking advantage of the isolated process 
                // architecture of Awesomium and let each view be rendered in a separate process.
                e.Cancel = true;
                // Note that we only explicitly navigate to the target URL, when a new view is 
                // about to be created, not when we wrap the created child view. This is because 
                // navigation to the target URL (if any), is already queued on created child views. 
                // We must not interrupt this navigation as we would still be breaking the parent-child
                // relationship.
                newWindow = new MainWindow( e.TargetURL );
                // Show the window.
                newWindow.Show();
            }
        }

        private void webControl_WindowClose( object sender, WindowCloseEventArgs e )
        {
            // The page called 'window.close'. If the call
            // comes from a frame, ignore it.
            if ( !e.IsCalledFromFrame )
                this.Close();
        }
        #endregion
    }
}
