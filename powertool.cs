using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Win32;
/*
Get the power profiles, show a dialog allowing the user to select one,
run powercfg to set it if the user clicks OK.  Compile: csc powertool.cs
*/
class powertool
{
  public static void doDlg()
  {
    int l1 = 0;//Radio Button Counter
    Form f1 = new Form();
    f1.FormBorderStyle = FormBorderStyle.FixedDialog;
    f1.ClientSize = new Size(320,240);

    Button buttonOK = new Button();
    buttonOK.Text = "OK";
    buttonOK.DialogResult = DialogResult.OK;
    buttonOK.Location = new Point(10,180);
    f1.Controls.Add(buttonOK);
    f1.AcceptButton = buttonOK;

    Button buttonCancel = new Button();
    buttonCancel.Text = "Cancel";
    buttonCancel.DialogResult = DialogResult.Cancel;
    buttonCancel.Location = new Point(100,180);
    f1.Controls.Add(buttonCancel);
    f1.CancelButton = buttonCancel;
    
    GroupBox groupBox1 = new GroupBox();
    groupBox1.Size = new Size(300,160);
    f1.Controls.Add(groupBox1);

    var regKey = @"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes";
    RegistryKey powerSchemesKey = Registry.LocalMachine.OpenSubKey(regKey);
    String activeKey = "";
    try
    {
      activeKey = (String)(powerSchemesKey.GetValue( "ActivePowerScheme" ));
    }
    catch( Exception ex )
    {
      Console.WriteLine( "Unable to get active power policy: " + ex.Message );
    }
    foreach( string sub in powerSchemesKey.GetSubKeyNames() )
    {
      RadioButton rb1 = new RadioButton();
      rb1.Text = sub;
      rb1.Name = sub;
      rb1.TabIndex = l1;
      groupBox1.Controls.Add( rb1 );
      if( sub == activeKey )
      {
        rb1.Checked = true;
        rb1.Select();
      }
      try
      {
        RegistryKey profileKey = powerSchemesKey.OpenSubKey( sub, false );
        String fn = (String)profileKey.GetValue( "FriendlyName" ); 
        if( fn[0] == '@' )
        {
          fn = fn.Substring( fn.IndexOf( ',' ) + 1 );
          fn = fn.Substring( fn.IndexOf( ',' ) + 1 );
        }
        rb1.Text = fn;
      }
      catch( Exception ex )
      {
        Console.WriteLine( "Unable to get profile name: " + ex.Message );
      }
      rb1.Location = new Point( 10, ++l1 * 30 );
      rb1.Size = new Size( 200, 26 );
    }//foreach( string sub in powerSchemesKey.GetSubKeyNames() )
    
    var res = f1.ShowDialog();
    if( res == DialogResult.OK )
    {
      foreach( RadioButton rb in groupBox1.Controls )
      {
        if( rb.Checked )
        {
          String cmdlExe = "powercfg";
          String cmdlParam = String.Format( "/setactive {0}", rb.Name );
          Console.WriteLine( cmdlExe + " " + cmdlParam );
          System.Diagnostics.Process.Start( cmdlExe, cmdlParam );
        }
      }
    }//if( res == DialogResult.OK )
  }//public static void doDlg()

  public static void Main(String[] args)
  {
    doDlg();
  }
}//class powertool
