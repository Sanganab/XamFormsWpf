<h2>Tutorial Overview</h2>
<b><span style="font-family:verdana,sans-serif">In this tutorial, we will create a cross-platform solution with Xamarin.Forms that includes WPF as a target.<br>
</span></b>
<div><b><span style="font-family:verdana,sans-serif"><br>
Cross-platform application development should reach the largest potential customer base it can.&nbsp; With Xamarin.Forms, you can create a solution with projects that target Android, iOS, Windows 10, Windows Mobile, Windows 8.1, and Windows Phone 8.1 out-of-the-box.&nbsp; Unfortunately, Windows Desktop is not included with this template.&nbsp; With so many people still using and relying on traditional Windows Desktop applications, many times it is important to provide a solution for that target environment.<br>
<br>
</span></b></div>
<div><b><span style="font-family:verdana,sans-serif">Fortunately, with only a few modifications, we can refactor our default Xamarin.Forms solution to include a WPF project.&nbsp; Furthermore, we will establish a project structure that ensures all the projects can still utilized common code (things like View Models, Services, Models) from a PCL project within the solution.</span></b><br>
<br>
</div>
<div><span style="font-family:verdana,sans-serif"><b>We will cover the following activities:</b></span><br>
</div>
<div>
<ul><li><span style="font-family:verdana,sans-serif"><b>Creating a new solution from the default Xamarin.Forms project template</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Adding a Core PCL to our solution, to contain our common code</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Add some common code to our Core PCL<br>
</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Using a simple IoC container to replace Xamarin.Forms DependencyService class</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Add a WPF project to our solution to target Windows Desktop</b></span></li>
<li><b><span style="font-family:verdana,sans-serif">Implementing IHelloService in each platform</span></b><br>
</li>
<li><span style="font-family:verdana,sans-serif"><b>Creating some simple data binding in our MainPage and MainWindow views<br>
</b></span></li></ul>
</div>
<div>
<hr>
<h2>Create the XamFormsWpf project<br>
</h2>
<h3><a name="TOC-Overview1"></a><span style="font-family:verdana,sans-serif">Overview</span></h3>
</div>
<div>
<p><b><span style="font-family:verdana,sans-serif">In this section,
 we will create a new solution that will contain all our common and 
platform-specific projects.&nbsp; We will create our solution using a built-in Xamarin Forms solution template.&nbsp; I will also briefly go over updating the NuGet dependencies once our solution is created.<br>
</span></b></p>
<h3><a name="TOC-Steps"></a><span style="font-family:verdana,sans-serif">Steps</span><br>
</h3>


<b><font face="verdana,sans-serif">Open a new instance of Visual Studio and select File &gt; New Project.</font></b><font face="verdana,sans-serif"><b>&nbsp; Search for 'Xamarin Forms' and select 'Blank Xaml App (Xamarin.Forms Portable)' option.<br>
<div style="display:block;text-align:left"></div>
Name the solution 'XamFormsWpf' and click OK.&nbsp; <br>
<div style="display:block;text-align:left"><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/01_CreateSolution.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/01_CreateSolution.PNG" style="width:100%" border="0"></a></div>
<br>
At some point during the initialization, you will be asked to determine which version of Windows 10 you want to target.&nbsp; Go ahead and take the defaults here.<br>
<br>
</b></font></div>
<div>
<h3><span style="font-family:verdana,sans-serif">A note about NuGet updates</span></h3>
<font face="verdana,sans-serif"><b>At the time of this writing, Xamarin Forms and the Android Support Library NuGet packages are out of sync.&nbsp; Updating Xamarin Forms downgrades the Android Support Libraries, and Upgrading these libraries downgrades Xamarin Forms.&nbsp; <br>
<br>
This results in a never-ending upgrade loop, with some random NuGet errors sprinkled in that you would probably prefer to avoid.&nbsp; My recommendation is to upgrade Xamarin.Forms, but leave the Android support libraries alone.</b></font><br>
<p><b><font size="2"><span style="font-family:verdana,sans-serif">From the Visual Studio menu, select Tools &gt; NuGet Package Manger &gt; Manage Packages for Solution. <br>
</span></font></b></p>
<p><b><font size="2"><span style="font-family:verdana,sans-serif">Select
 the 'Updates' tab, and check the checkboxes for 'Microsoft.NETCore.UniversalWindowsPlatform' and 'Xamarin.Forms'<br>
</span></font></b></p>
<p><b><font size="2"><span style="font-family:verdana,sans-serif">*Note - the specific packages to update may be different, as these change over time.<br>
</span></font></b></p>
<p><b><font size="2"><span style="font-family:verdana,sans-serif">Here is a screenshot of my available Updates at the time of this writing.</span></font></b></p>
<div style="display:block;text-align:left"><b><font size="2"><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/02_NuGetUpdates.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/02_NuGetUpdates.PNG" style="width:100%" border="0"></a></font></b></div>
<br>
<hr>
<h2>Adding a Core PCL to our solution, to contain our common code<br>
</h2>
<h3><a name="TOC-Overview2"></a><span style="font-family:verdana,sans-serif">Overview</span></h3>
<p><span style="font-family:verdana,sans-serif"><b>When we created our solution from the Xamarin.Forms template, it created a PCL called XamFormsWpf (Portable).&nbsp; If you are coming from a Xamarin.Android or Xamarin.iOS background, you might expect this project to contain things like View Models, Models, Services, etc.&nbsp; <br>
</b></span></p>
<p><span style="font-family:verdana,sans-serif"><b>With Xamarin.Forms, this PCL actually contains our common View definitions as well.&nbsp; Since we cannot reuse the Xamarin.Forms XAML in our WPF project, we will need to create another PCL to hold our common core code.</b></span></p>
<h3><a name="TOC-Steps2"></a><span style="font-family:verdana,sans-serif">Steps</span></h3>
<p><span style="font-family:verdana,sans-serif"><b>Right-click the XamFormsWpf (Portable) project and select 'Rename'.&nbsp; Change the name to XamFormsWpf.Core.Forms. <br>
</b></span></p>
<p><span style="font-family:verdana,sans-serif"><b>Right-click the Solution and select Add &gt; New Project.&nbsp; Search for 'Portable' and select 'Class Library (Portable for iOS, Android, and Windows)'<br>
</b></span></p>
<div style="display:block;text-align:left"><b><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/04_CreatePCL.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/04_CreatePCL.PNG" style="width:100%" border="0"></a></b></div>
<b>Name the new project XamFormsWpf.Core and click OK.</b>
<p><span style="font-family:verdana,sans-serif"><b>Add references to the new PCL.&nbsp; Right-click References &gt; Add References.&nbsp; Under Projects check the check box for XamFormsWpf.Core and click OK.&nbsp; Do this for each project in the solution.<br>
</b></span></p>
<p><span style="font-family:verdana,sans-serif"><b>Once you are finished, go ahead and build to make sure everything is properly configured.&nbsp; Your solution structure should now look like this</b></span></p>
<div style="display:block;text-align:left"><b><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/05_SolutionStructure.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/05_SolutionStructure.PNG" border="0"></a></b></div>
<br>
<hr>
<h2>Add some common code to our Core PCL<br>
</h2>
<h3><a name="TOC-Overview2"></a><span style="font-family:verdana,sans-serif">Overview</span></h3>
<p><span style="font-family:verdana,sans-serif"><b>The next step is to create some common code in our Core PCL to demonstrate how our Xamarin.Forms and WPF project will utilize it.&nbsp; <br>
</b></span></p>
<p><span style="font-family:verdana,sans-serif"><b>As our example, we will create a single Contact page for each of our apps.&nbsp; The page will display a contact's First Name, Last Name, and Email Address.&nbsp; Since Xamarin.Forms DependencyService class won't be available in WPF, we'll need a substitute service locator.&nbsp; We will create a simple Hello Service interface, with implementations in our Forms and WPF projects, what we'll wire up with simple IoC replacement for DependencyService.</b></span></p>
<h3><span style="font-family:verdana,sans-serif">Steps</span></h3>
<p><span style="font-family:verdana,sans-serif"><b>Delete Class1.cs, we won't need the functionality it provides.<br>
</b></span></p>
<p><span style="font-family:verdana,sans-serif"><b>In XamFormsWpf.Core:</b></span></p>
<p><span style="font-family:verdana,sans-serif"><b>Create a new folder called Data, add a class to the folder called 'Contact' with the following code.</b></span><br>
</p>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.Core.Data
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">class</span> Contact
    {
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> FirstName { get; <span style="color:rgb(0,134,179)">set</span>; }
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> LastName { get; <span style="color:rgb(0,134,179)">set</span>; }
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> Email { get; <span style="color:rgb(0,134,179)">set</span>; }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPm5hbWVzcGFjZSBYYW1Gb3Jtc1dwZi5Db3JlLkRhdGE8YnI+ezxicj7CoMKgwqAg cHVibGljIGNsYXNzIENvbnRhY3Q8YnI+wqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqAgcHVibGlj IHN0cmluZyBGaXJzdE5hbWUgeyBnZXQ7IHNldDsgfTxicj7CoMKgwqDCoMKgwqDCoCBwdWJsaWMg c3RyaW5nIExhc3ROYW1lIHsgZ2V0OyBzZXQ7IH08YnI+wqDCoMKgwqDCoMKgwqAgcHVibGljIHN0 cmluZyBFbWFpbCB7IGdldDsgc2V0OyB9PGJyPsKgwqDCoCB9PGJyPn08YnI+PGJyPgpgYGA=">​</div>
</div>
<p><span style="font-family:verdana,sans-serif"><b>Create a new folder called Services, add a new interface to the folder called 'IHelloService' with the following code</b></span></p>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.Core.Services
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> interface IHelloService
    {
        <span style="color:rgb(0,134,179)">string</span> SayHello();
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPm5hbWVzcGFjZSBYYW1Gb3Jtc1dwZi5Db3JlLlNlcnZpY2VzPGJyPns8YnI+wqDC oMKgIHB1YmxpYyBpbnRlcmZhY2UgSUhlbGxvU2VydmljZTxicj7CoMKgwqAgezxicj7CoMKgwqDC oMKgwqDCoCBzdHJpbmcgU2F5SGVsbG8oKTs8YnI+wqDCoMKgIH08YnI+fTxicj48YnI+CmBgYA==">​</div>
</div>
<p><b><span style="font-family:verdana,sans-serif">In the Services folder, create a new class called SimpleIoC with the following code.&nbsp; <br>
</span></b></p>
<p><b><span style="font-family:verdana,sans-serif">There are many IoC containers that are available for use with Xamarin projects, but for the purpose of this demo, I decided to create a quick one myself.&nbsp; ***This container IS NOT meant for production use, it was created to be simple enough to reduce the learning overhead of working without DependencyService.  This will replace Xamarin.Forms DependencyService class, which isn't available in WPF, and will wire up our IHelloService interface with an implementation in our Forms and WPF projects later.</span></b></p>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> System;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> System.Collections.Generic;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.Core.Services
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> sealed <span style="color:rgb(51,51,51);font-weight:bold">class</span> SimpleIoC
    {
        <span style="color:rgb(51,51,51);font-weight:bold">private</span> <span style="color:rgb(51,51,51);font-weight:bold">static</span> <span style="color:rgb(51,51,51);font-weight:bold">volatile</span> SimpleIoC _instance;
        <span style="color:rgb(51,51,51);font-weight:bold">private</span> <span style="color:rgb(51,51,51);font-weight:bold">static</span> object _syncRoot = <span style="color:rgb(51,51,51);font-weight:bold">new</span> object();
        <span style="color:rgb(51,51,51);font-weight:bold">private</span> Dictionary&lt;Type, Type&gt; _multiInstance = <span style="color:rgb(51,51,51);font-weight:bold">new</span> Dictionary&lt;Type, Type&gt;();
        <span style="color:rgb(51,51,51);font-weight:bold">private</span> Dictionary&lt;Type, object&gt; _singletons = <span style="color:rgb(51,51,51);font-weight:bold">new</span> Dictionary&lt;Type, object&gt;();

        <span style="color:rgb(51,51,51);font-weight:bold">private</span> SimpleIoC() { }
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">static</span> SimpleIoC Container
        {
            get {
                <span style="color:rgb(51,51,51);font-weight:bold">if</span>(_instance == null) {
                    lock(_syncRoot) {
                        <span style="color:rgb(51,51,51);font-weight:bold">if</span>(_instance == null)
                            _instance = <span style="color:rgb(51,51,51);font-weight:bold">new</span> SimpleIoC();
                    }
                }

                <span style="color:rgb(51,51,51);font-weight:bold">return</span> _instance;
            }
        }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> Register&lt;Implementation&gt;()
        {
            RegisterSingleton&lt;Implementation, Implementation&gt;();
        }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> RegisterSingleton&lt;Implementation&gt;()
        {
            RegisterSingleton&lt;Implementation, Implementation&gt;();
        }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> Register&lt;Interface, Implementation&gt;()
        {
            Validate&lt;Interface, Implementation&gt;();
            _multiInstance.Add(typeof(Interface), typeof(Implementation));
        }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> RegisterSingleton&lt;Interface, Implementation&gt;()
        {
            Validate&lt;Interface, Implementation&gt;();
            _singletons.Add(typeof(Interface), Activator.CreateInstance(typeof(Implementation)));
        }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> Type Resolve&lt;Type&gt;()
        {
            <span style="color:rgb(51,51,51);font-weight:bold">if</span>(_singletons.ContainsKey(typeof(Type))) {
                <span style="color:rgb(51,51,51);font-weight:bold">return</span> (Type)_singletons[typeof(Type)];
            }
            <span style="color:rgb(51,51,51);font-weight:bold">else</span> <span style="color:rgb(51,51,51);font-weight:bold">if</span>(_multiInstance.ContainsKey(typeof(Type))) {
                var theType = _multiInstance[typeof(Type)];
                var obj = Activator.CreateInstance(theType);
                <span style="color:rgb(51,51,51);font-weight:bold">return</span> (Type)obj;
            }
            <span style="color:rgb(51,51,51);font-weight:bold">else</span>
                <span style="color:rgb(51,51,51);font-weight:bold">throw</span> <span style="color:rgb(51,51,51);font-weight:bold">new</span> Exception($<span style="color:rgb(221,17,68)">"Type {typeof(Type).ToString()} is not registered"</span>);
        }

        <span style="color:rgb(51,51,51);font-weight:bold">private</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> Validate&lt;Interface, Implementation&gt;()
        {
            <span style="color:rgb(153,153,136);font-style:italic">// This will fail up-front if the class cannot be instantiated correctly</span>
            Activator.CreateInstance&lt;Implementation&gt;();

            <span style="color:rgb(51,51,51);font-weight:bold">if</span>(_multiInstance.ContainsKey(typeof(Interface))) {
                 <span style="color:rgb(51,51,51);font-weight:bold">throw</span> <span style="color:rgb(51,51,51);font-weight:bold">new</span> Exception($<span style="color:rgb(221,17,68)">"Type  {_multiInstance[typeof(Interface)].ToString()} is already registered for  {typeof(Interface).ToString()}."</span>);
            }
            <span style="color:rgb(51,51,51);font-weight:bold">else</span> <span style="color:rgb(51,51,51);font-weight:bold">if</span>(_singletons.ContainsKey(typeof(Interface))) {
                 <span style="color:rgb(51,51,51);font-weight:bold">throw</span> <span style="color:rgb(51,51,51);font-weight:bold">new</span> Exception($<span style="color:rgb(221,17,68)">"Type {_singletons[typeof(Interface)].ToString()}  is already registered for {typeof(Interface).ToString()}."</span>);
            }
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFN5c3RlbTs8YnI+dXNpbmcgU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVy aWM7PGJyPjxicj5uYW1lc3BhY2UgWGFtRm9ybXNXcGYuQ29yZS5TZXJ2aWNlczxicj57PGJyPsKg wqDCoCBwdWJsaWMgc2VhbGVkIGNsYXNzIFNpbXBsZUlvQzxicj7CoMKgwqAgezxicj7CoMKgwqDC oMKgwqDCoCBwcml2YXRlIHN0YXRpYyB2b2xhdGlsZSBTaW1wbGVJb0MgX2luc3RhbmNlOzxicj7C oMKgwqDCoMKgwqDCoCBwcml2YXRlIHN0YXRpYyBvYmplY3QgX3N5bmNSb290ID0gbmV3IG9iamVj dCgpOzxicj7CoMKgwqDCoMKgwqDCoCBwcml2YXRlIERpY3Rpb25hcnkmbHQ7VHlwZSwgVHlwZSZn dDsgX211bHRpSW5zdGFuY2UgPSBuZXcgRGljdGlvbmFyeSZsdDtUeXBlLCBUeXBlJmd0OygpOzxi cj7CoMKgwqDCoMKgwqDCoCBwcml2YXRlIERpY3Rpb25hcnkmbHQ7VHlwZSwgb2JqZWN0Jmd0OyBf c2luZ2xldG9ucyA9IG5ldyBEaWN0aW9uYXJ5Jmx0O1R5cGUsIG9iamVjdCZndDsoKTs8YnI+PGJy PsKgwqDCoMKgwqDCoMKgIHByaXZhdGUgU2ltcGxlSW9DKCkgeyB9PGJyPsKgwqDCoMKgwqDCoMKg IHB1YmxpYyBzdGF0aWMgU2ltcGxlSW9DIENvbnRhaW5lcjxicj7CoMKgwqDCoMKgwqDCoCB7PGJy PsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgZ2V0IHs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKg wqDCoMKgIGlmKF9pbnN0YW5jZSA9PSBudWxsKSB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDC oMKgwqDCoMKgwqDCoMKgIGxvY2soX3N5bmNSb290KSB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKg wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgaWYoX2luc3RhbmNlID09IG51bGwpPGJyPsKgwqDC oMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBfaW5zdGFu Y2UgPSBuZXcgU2ltcGxlSW9DKCk7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKg wqDCoMKgIH08YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgIH08YnI+PGJyPsKgwqDC oMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCByZXR1cm4gX2luc3RhbmNlOzxicj7CoMKgwqDCoMKg wqDCoMKgwqDCoMKgIH08YnI+wqDCoMKgwqDCoMKgwqAgfTxicj48YnI+wqDCoMKgwqDCoMKgwqAg cHVibGljIHZvaWQgUmVnaXN0ZXImbHQ7SW1wbGVtZW50YXRpb24mZ3Q7KCk8YnI+wqDCoMKgwqDC oMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIFJlZ2lzdGVyU2luZ2xldG9uJmx0O0lt cGxlbWVudGF0aW9uLCBJbXBsZW1lbnRhdGlvbiZndDsoKTs8YnI+wqDCoMKgwqDCoMKgwqAgfTxi cj48YnI+wqDCoMKgwqDCoMKgwqAgcHVibGljIHZvaWQgUmVnaXN0ZXJTaW5nbGV0b24mbHQ7SW1w bGVtZW50YXRpb24mZ3Q7KCk8YnI+wqDCoMKgwqDCoMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoMKg wqDCoMKgIFJlZ2lzdGVyU2luZ2xldG9uJmx0O0ltcGxlbWVudGF0aW9uLCBJbXBsZW1lbnRhdGlv biZndDsoKTs8YnI+wqDCoMKgwqDCoMKgwqAgfTxicj48YnI+wqDCoMKgwqDCoMKgwqAgcHVibGlj IHZvaWQgUmVnaXN0ZXImbHQ7SW50ZXJmYWNlLCBJbXBsZW1lbnRhdGlvbiZndDsoKTxicj7CoMKg wqDCoMKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgVmFsaWRhdGUmbHQ7SW50ZXJm YWNlLCBJbXBsZW1lbnRhdGlvbiZndDsoKTs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBfbXVs dGlJbnN0YW5jZS5BZGQodHlwZW9mKEludGVyZmFjZSksIHR5cGVvZihJbXBsZW1lbnRhdGlvbikp Ozxicj7CoMKgwqDCoMKgwqDCoCB9PGJyPjxicj7CoMKgwqDCoMKgwqDCoCBwdWJsaWMgdm9pZCBS ZWdpc3RlclNpbmdsZXRvbiZsdDtJbnRlcmZhY2UsIEltcGxlbWVudGF0aW9uJmd0OygpPGJyPsKg wqDCoMKgwqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBWYWxpZGF0ZSZsdDtJbnRl cmZhY2UsIEltcGxlbWVudGF0aW9uJmd0OygpOzxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIF9z aW5nbGV0b25zLkFkZCh0eXBlb2YoSW50ZXJmYWNlKSwgQWN0aXZhdG9yLkNyZWF0ZUluc3RhbmNl KHR5cGVvZihJbXBsZW1lbnRhdGlvbikpKTs8YnI+wqDCoMKgwqDCoMKgwqAgfTxicj48YnI+wqDC oMKgwqDCoMKgwqAgcHVibGljIFR5cGUgUmVzb2x2ZSZsdDtUeXBlJmd0OygpPGJyPsKgwqDCoMKg wqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBpZihfc2luZ2xldG9ucy5Db250YWlu c0tleSh0eXBlb2YoVHlwZSkpKSB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBy ZXR1cm4gKFR5cGUpX3NpbmdsZXRvbnNbdHlwZW9mKFR5cGUpXTs8YnI+wqDCoMKgwqDCoMKgwqDC oMKgwqDCoCB9PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgZWxzZSBpZihfbXVsdGlJbnN0YW5j ZS5Db250YWluc0tleSh0eXBlb2YoVHlwZSkpKSB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDC oMKgwqDCoCB2YXIgdGhlVHlwZSA9IF9tdWx0aUluc3RhbmNlW3R5cGVvZihUeXBlKV07PGJyPsKg wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCB2YXIgb2JqID0gQWN0aXZhdG9yLkNyZWF0ZUlu c3RhbmNlKHRoZVR5cGUpOzxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgcmV0dXJu IChUeXBlKW9iajs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCB9PGJyPsKgwqDCoMKgwqDCoMKg wqDCoMKgwqAgZWxzZTxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgdGhyb3cgbmV3 IEV4Y2VwdGlvbigkIlR5cGUge3R5cGVvZihUeXBlKS5Ub1N0cmluZygpfSBpcyBub3QgcmVnaXN0 ZXJlZCIpOzxicj7CoMKgwqDCoMKgwqDCoCB9PGJyPjxicj7CoMKgwqDCoMKgwqDCoCBwcml2YXRl IHZvaWQgVmFsaWRhdGUmbHQ7SW50ZXJmYWNlLCBJbXBsZW1lbnRhdGlvbiZndDsoKTxicj7CoMKg wqDCoMKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgLy8gVGhpcyB3aWxsIGZhaWwg dXAtZnJvbnQgaWYgdGhlIGNsYXNzIGNhbm5vdCBiZSBpbnN0YW50aWF0ZWQgY29ycmVjdGx5PGJy PsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgQWN0aXZhdG9yLkNyZWF0ZUluc3RhbmNlJmx0O0ltcGxl bWVudGF0aW9uJmd0OygpOzxicj48YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBpZihfbXVsdGlJ bnN0YW5jZS5Db250YWluc0tleSh0eXBlb2YoSW50ZXJmYWNlKSkpIHs8YnI+wqDCoMKgwqDCoMKg wqDCoMKgwqDCoMKgwqDCoMKgCiB0aHJvdyBuZXcgRXhjZXB0aW9uKCQiVHlwZSAKe19tdWx0aUlu c3RhbmNlW3R5cGVvZihJbnRlcmZhY2UpXS5Ub1N0cmluZygpfSBpcyBhbHJlYWR5IHJlZ2lzdGVy ZWQgZm9yCiB7dHlwZW9mKEludGVyZmFjZSkuVG9TdHJpbmcoKX0uIik7PGJyPsKgwqDCoMKgwqDC oMKgwqDCoMKgwqAgfTxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIGVsc2UgaWYoX3NpbmdsZXRv bnMuQ29udGFpbnNLZXkodHlwZW9mKEludGVyZmFjZSkpKSB7PGJyPsKgwqDCoMKgwqDCoMKgwqDC oMKgwqDCoMKgwqDCoAogdGhyb3cgbmV3IEV4Y2VwdGlvbigkIlR5cGUge19zaW5nbGV0b25zW3R5 cGVvZihJbnRlcmZhY2UpXS5Ub1N0cmluZygpfSAKaXMgYWxyZWFkeSByZWdpc3RlcmVkIGZvciB7 dHlwZW9mKEludGVyZmFjZSkuVG9TdHJpbmcoKX0uIik7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKg wqAgfTxicj7CoMKgwqDCoMKgwqDCoCB9PGJyPsKgwqDCoCB9PGJyPn08YnI+CmBgYA==">​</div>
</div>
<p><b><span style="font-family:verdana,sans-serif">Create a new folder called ViewModels, add a new class to the folder called 'BaseViewModel' with the following code.&nbsp; This class will implement INotifyPropertyChanged for our data bound properties in Xamarin.Forms and WPF.</span></b></p>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> System.ComponentModel;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> System.Runtime.CompilerServices;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.Core.ViewModels
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">class</span> BaseViewModel : INotifyPropertyChanged
    {
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> event PropertyChangedEventHandler PropertyChanged = delegate { };
        <span style="color:rgb(51,51,51);font-weight:bold">protected</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> RaisePropertyChanged([CallerMemberName] <span style="color:rgb(0,134,179)">string</span> propertyName = <span style="color:rgb(221,17,68)">""</span>)
        {
            PropertyChanged(<span style="color:rgb(51,51,51);font-weight:bold">this</span>, <span style="color:rgb(51,51,51);font-weight:bold">new</span> PropertyChangedEventArgs(propertyName));
        }

        <span style="color:rgb(51,51,51);font-weight:bold">protected</span> <span style="color:rgb(51,51,51);font-weight:bold">void</span> RaiseAllPropertiesChanged()
        {
            PropertyChanged(<span style="color:rgb(51,51,51);font-weight:bold">this</span>, null);
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFN5c3RlbS5Db21wb25lbnRNb2RlbDs8YnI+dXNpbmcgU3lzdGVtLlJ1 bnRpbWUuQ29tcGlsZXJTZXJ2aWNlczs8YnI+PGJyPm5hbWVzcGFjZSBYYW1Gb3Jtc1dwZi5Db3Jl LlZpZXdNb2RlbHM8YnI+ezxicj7CoMKgwqAgcHVibGljIGNsYXNzIEJhc2VWaWV3TW9kZWwgOiBJ Tm90aWZ5UHJvcGVydHlDaGFuZ2VkPGJyPsKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgIHB1Ymxp YyBldmVudCBQcm9wZXJ0eUNoYW5nZWRFdmVudEhhbmRsZXIgUHJvcGVydHlDaGFuZ2VkID0gZGVs ZWdhdGUgeyB9Ozxicj7CoMKgwqDCoMKgwqDCoCBwcm90ZWN0ZWQgdm9pZCBSYWlzZVByb3BlcnR5 Q2hhbmdlZChbQ2FsbGVyTWVtYmVyTmFtZV0gc3RyaW5nIHByb3BlcnR5TmFtZSA9ICIiKTxicj7C oMKgwqDCoMKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgUHJvcGVydHlDaGFuZ2Vk KHRoaXMsIG5ldyBQcm9wZXJ0eUNoYW5nZWRFdmVudEFyZ3MocHJvcGVydHlOYW1lKSk7PGJyPsKg wqDCoMKgwqDCoMKgIH08YnI+PGJyPsKgwqDCoMKgwqDCoMKgIHByb3RlY3RlZCB2b2lkIFJhaXNl QWxsUHJvcGVydGllc0NoYW5nZWQoKTxicj7CoMKgwqDCoMKgwqDCoCB7PGJyPsKgwqDCoMKgwqDC oMKgwqDCoMKgwqAgUHJvcGVydHlDaGFuZ2VkKHRoaXMsIG51bGwpOzxicj7CoMKgwqDCoMKgwqDC oCB9PGJyPsKgwqDCoCB9PGJyPn08YnI+CmBgYA==">​</div>
</div>
<p><b><span style="font-family:verdana,sans-serif">Now add the ContactViewModel class to the ViewModels folder with the following code.&nbsp; This is the class we will bind our forms to in WPF and Xamarin.Forms.&nbsp; Notice that this class uses our IoC container to resolve the IHelloService at runtime.&nbsp; The class also uses our Contact class as a backing object for our data bound contact properties.</span></b></p>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Data;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Services;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.Core.ViewModels
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(51,51,51);font-weight:bold">class</span> ContactViewModel : BaseViewModel
    {
        <span style="color:rgb(51,51,51);font-weight:bold">private</span> Contact _contact;
        <span style="color:rgb(51,51,51);font-weight:bold">private</span> IHelloService _helloService = SimpleIoC.Container.Resolve&lt;IHelloService&gt;();

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> Hello { get { <span style="color:rgb(51,51,51);font-weight:bold">return</span> _helloService.SayHello(); } }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> ContactFirstName
        {
            get { <span style="color:rgb(51,51,51);font-weight:bold">return</span> _contact.FirstName; }
            <span style="color:rgb(0,134,179)">set</span> {
                _contact.FirstName = value;
                RaisePropertyChanged(nameof(ContactFirstName));
            }
        }
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> ContactLastName
        {
            get { <span style="color:rgb(51,51,51);font-weight:bold">return</span> _contact.LastName; }
            <span style="color:rgb(0,134,179)">set</span> {
                _contact.LastName = value;
                RaisePropertyChanged(nameof(ContactLastName));
            }
        }
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> ContactEmail
        {
            get { <span style="color:rgb(51,51,51);font-weight:bold">return</span> _contact.Email; }
            <span style="color:rgb(0,134,179)">set</span> {
                _contact.Email = value;
                RaisePropertyChanged(nameof(ContactEmail));
            }
        }

        <span style="color:rgb(51,51,51);font-weight:bold">public</span> ContactViewModel(Contact contact = null)
        {
            _contact = contact == null ? <span style="color:rgb(51,51,51);font-weight:bold">new</span> Contact() : contact;
            RaiseAllPropertiesChanged();
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFhhbUZvcm1zV3BmLkNvcmUuRGF0YTs8YnI+dXNpbmcgWGFtRm9ybXNX cGYuQ29yZS5TZXJ2aWNlczs8YnI+PGJyPm5hbWVzcGFjZSBYYW1Gb3Jtc1dwZi5Db3JlLlZpZXdN b2RlbHM8YnI+ezxicj7CoMKgwqAgcHVibGljIGNsYXNzIENvbnRhY3RWaWV3TW9kZWwgOiBCYXNl Vmlld01vZGVsPGJyPsKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgIHByaXZhdGUgQ29udGFjdCBf Y29udGFjdDs8YnI+wqDCoMKgwqDCoMKgwqAgcHJpdmF0ZSBJSGVsbG9TZXJ2aWNlIF9oZWxsb1Nl cnZpY2UgPSBTaW1wbGVJb0MuQ29udGFpbmVyLlJlc29sdmUmbHQ7SUhlbGxvU2VydmljZSZndDso KTs8YnI+PGJyPjxicj7CoMKgwqDCoMKgwqDCoCBwdWJsaWMgc3RyaW5nIEhlbGxvIHsgZ2V0IHsg cmV0dXJuIF9oZWxsb1NlcnZpY2UuU2F5SGVsbG8oKTsgfSB9PGJyPjxicj7CoMKgwqDCoMKgwqDC oCBwdWJsaWMgc3RyaW5nIENvbnRhY3RGaXJzdE5hbWU8YnI+wqDCoMKgwqDCoMKgwqAgezxicj7C oMKgwqDCoMKgwqDCoMKgwqDCoMKgIGdldCB7IHJldHVybiBfY29udGFjdC5GaXJzdE5hbWU7IH08 YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBzZXQgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKg wqDCoMKgwqAgX2NvbnRhY3QuRmlyc3ROYW1lID0gdmFsdWU7PGJyPsKgwqDCoMKgwqDCoMKgwqDC oMKgwqDCoMKgwqDCoCBSYWlzZVByb3BlcnR5Q2hhbmdlZChuYW1lb2YoQ29udGFjdEZpcnN0TmFt ZSkpOzxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIH08YnI+wqDCoMKgwqDCoMKgwqAgfTxicj7C oMKgwqDCoMKgwqDCoCBwdWJsaWMgc3RyaW5nIENvbnRhY3RMYXN0TmFtZTxicj7CoMKgwqDCoMKg wqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgZ2V0IHsgcmV0dXJuIF9jb250YWN0Lkxh c3ROYW1lOyB9PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgc2V0IHs8YnI+wqDCoMKgwqDCoMKg wqDCoMKgwqDCoMKgwqDCoMKgIF9jb250YWN0Lkxhc3ROYW1lID0gdmFsdWU7PGJyPsKgwqDCoMKg wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBSYWlzZVByb3BlcnR5Q2hhbmdlZChuYW1lb2YoQ29udGFj dExhc3ROYW1lKSk7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgfTxicj7CoMKgwqDCoMKgwqDC oCB9PGJyPsKgwqDCoMKgwqDCoMKgIHB1YmxpYyBzdHJpbmcgQ29udGFjdEVtYWlsPGJyPsKgwqDC oMKgwqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBnZXQgeyByZXR1cm4gX2NvbnRh Y3QuRW1haWw7IH08YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBzZXQgezxicj7CoMKgwqDCoMKg wqDCoMKgwqDCoMKgwqDCoMKgwqAgX2NvbnRhY3QuRW1haWwgPSB2YWx1ZTs8YnI+wqDCoMKgwqDC oMKgwqDCoMKgwqDCoMKgwqDCoMKgIFJhaXNlUHJvcGVydHlDaGFuZ2VkKG5hbWVvZihDb250YWN0 RW1haWwpKTs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCB9PGJyPsKgwqDCoMKgwqDCoMKgIH08 YnI+PGJyPsKgwqDCoMKgwqDCoMKgIHB1YmxpYyBDb250YWN0Vmlld01vZGVsKENvbnRhY3QgY29u dGFjdCA9IG51bGwpPGJyPsKgwqDCoMKgwqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDC oCBfY29udGFjdCA9IGNvbnRhY3QgPT0gbnVsbCA/IG5ldyBDb250YWN0KCkgOiBjb250YWN0Ozxi cj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIFJhaXNlQWxsUHJvcGVydGllc0NoYW5nZWQoKTs8YnI+ wqDCoMKgwqDCoMKgwqAgfTxicj7CoMKgwqAgfTxicj59PGJyPgpgYGA=">​</div>
</div>
<p><b><span style="font-family:verdana,sans-serif">This is everything we'll need in our Core PCL in order to run the example.&nbsp; We still have a couple more steps in our View projects to wire up our Views with the View Models and Services, but we'll take care of that next.&nbsp; Below is a screenshot of what your XamFormsWpf.Core project should look like in the solution window.</span></b></p>
<div style="display:block;text-align:left"><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/06_CoreFinished.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/06_CoreFinished.PNG" border="0"></a></div>
<br>
<hr>
<h2>Add a WPF project to our solution to target Windows Desktop<br>
</h2>
<h3><a name="TOC-Overview2"></a><span style="font-family:verdana,sans-serif">Overview</span></h3>
<span style="font-family:verdana,sans-serif"><b>Now it is time to add our WPF project to to the soluion.&nbsp; In this step, we will add a standard WPF project, and have it reference the XamFormsWpf.Core PCL.<br>
<br>
</b></span>
<h3><span style="font-family:verdana,sans-serif">Steps</span></h3>
<p><span style="font-family:verdana,sans-serif"><b>Right click the solution name and select Add &gt; New Project.&nbsp; Search for WPF and select WPF Application.&nbsp; Make sure you select the C# template.&nbsp; Name the project XamFormsWpf.WPF.<br>
</b></span></p>
<div style="display:block;text-align:left"><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/07_WpfProjectCreate.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/07_WpfProjectCreate.PNG" style="width:100%" border="0"></a></div>
<br>
<p><b><span style="font-family:verdana,sans-serif">In the new WPF project, add a reference to the XamFormsWpf.Core (Portable) project.</span></b></p>
<hr>
<h2>Implementing IHelloService in each platform<br>
</h2>
<h3><a name="TOC-Overview2"></a><span style="font-family:verdana,sans-serif">Overview</span></h3>
<span style="font-family:verdana,sans-serif"><b>Now we will create a HelloService class in the WPF and Forms projects that provides a custom implementation for WPF and Forms.&nbsp; We will register this class in each project's App.cs using our SimpleIoC container that replaced DependencyService.<br>
<br>
</b></span>
<h3><span style="font-family:verdana,sans-serif">Steps</span></h3>
<span style="font-family:verdana,sans-serif"><b>Create a new class called HelloService in the XamFormsWpf.Core.Forms project with the following code</b></span><br>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Services;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf
{
    <span style="color:rgb(51,51,51);font-weight:bold">class</span> HelloService : IHelloService
    {
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> SayHello()
        {
            <span style="color:rgb(51,51,51);font-weight:bold">return</span> <span style="color:rgb(221,17,68)">"Hello, Xamarin Forms!"</span>;
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFhhbUZvcm1zV3BmLkNvcmUuU2VydmljZXM7PGJyPjxicj5uYW1lc3Bh Y2UgWGFtRm9ybXNXcGY8YnI+ezxicj7CoMKgwqAgY2xhc3MgSGVsbG9TZXJ2aWNlIDogSUhlbGxv U2VydmljZTxicj7CoMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoCBwdWJsaWMgc3RyaW5nIFNheUhl bGxvKCk8YnI+wqDCoMKgwqDCoMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIHJldHVy biAiSGVsbG8sIFhhbWFyaW4gRm9ybXMhIjs8YnI+wqDCoMKgwqDCoMKgwqAgfTxicj7CoMKgwqAg fTxicj59PGJyPgpgYGA=">​</div>
</div>
<b><span style="font-family:verdana,sans-serif">Update the App.xaml.cs class in XamFormsWpf.Core.Forms to use our SimpleIoC container to register our HelloWorld service as the implementation.&nbsp; Replace the App.xaml.cs code with the following code</span></b><br>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Services;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> Xamarin.Forms;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> partial <span style="color:rgb(51,51,51);font-weight:bold">class</span> App : Application
    {
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> SimpleIoC IoC = SimpleIoC.Container;
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> App()
        {
            IoC.RegisterSingleton&lt;IHelloService, HelloService&gt;();
            InitializeComponent();
            MainPage = <span style="color:rgb(51,51,51);font-weight:bold">new</span> MainPage();
        }

        <span style="color:rgb(51,51,51);font-weight:bold">protected</span> override <span style="color:rgb(51,51,51);font-weight:bold">void</span> OnStart()
        {
            <span style="color:rgb(153,153,136);font-style:italic">// Handle when your app starts</span>
        }

        <span style="color:rgb(51,51,51);font-weight:bold">protected</span> override <span style="color:rgb(51,51,51);font-weight:bold">void</span> OnSleep()
        {
            <span style="color:rgb(153,153,136);font-style:italic">// Handle when your app sleeps</span>
        }

        <span style="color:rgb(51,51,51);font-weight:bold">protected</span> override <span style="color:rgb(51,51,51);font-weight:bold">void</span> OnResume()
        {
            <span style="color:rgb(153,153,136);font-style:italic">// Handle when your app resumes</span>
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFhhbUZvcm1zV3BmLkNvcmUuU2VydmljZXM7PGJyPnVzaW5nIFhhbWFy aW4uRm9ybXM7PGJyPjxicj5uYW1lc3BhY2UgWGFtRm9ybXNXcGY8YnI+ezxicj7CoMKgwqAgcHVi bGljIHBhcnRpYWwgY2xhc3MgQXBwIDogQXBwbGljYXRpb248YnI+wqDCoMKgIHs8YnI+wqDCoMKg wqDCoMKgwqAgcHVibGljIFNpbXBsZUlvQyBJb0MgPSBTaW1wbGVJb0MuQ29udGFpbmVyOzxicj7C oMKgwqDCoMKgwqDCoCBwdWJsaWMgQXBwKCk8YnI+wqDCoMKgwqDCoMKgwqAgezxicj7CoMKgwqDC oMKgwqDCoMKgwqDCoMKgIElvQy5SZWdpc3RlclNpbmdsZXRvbiZsdDtJSGVsbG9TZXJ2aWNlLCBI ZWxsb1NlcnZpY2UmZ3Q7KCk7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgSW5pdGlhbGl6ZUNv bXBvbmVudCgpOzxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIE1haW5QYWdlID0gbmV3IE1haW5Q YWdlKCk7PGJyPsKgwqDCoMKgwqDCoMKgIH08YnI+PGJyPsKgwqDCoMKgwqDCoMKgIHByb3RlY3Rl ZCBvdmVycmlkZSB2b2lkIE9uU3RhcnQoKTxicj7CoMKgwqDCoMKgwqDCoCB7PGJyPsKgwqDCoMKg wqDCoMKgwqDCoMKgwqAgLy8gSGFuZGxlIHdoZW4geW91ciBhcHAgc3RhcnRzPGJyPsKgwqDCoMKg wqDCoMKgIH08YnI+PGJyPsKgwqDCoMKgwqDCoMKgIHByb3RlY3RlZCBvdmVycmlkZSB2b2lkIE9u U2xlZXAoKTxicj7CoMKgwqDCoMKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgLy8g SGFuZGxlIHdoZW4geW91ciBhcHAgc2xlZXBzPGJyPsKgwqDCoMKgwqDCoMKgIH08YnI+PGJyPsKg wqDCoMKgwqDCoMKgIHByb3RlY3RlZCBvdmVycmlkZSB2b2lkIE9uUmVzdW1lKCk8YnI+wqDCoMKg wqDCoMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIC8vIEhhbmRsZSB3aGVuIHlvdXIg YXBwIHJlc3VtZXM8YnI+wqDCoMKgwqDCoMKgwqAgfTxicj7CoMKgwqAgfTxicj59PGJyPgpgYGA=">​</div>
</div>
<b><span style="font-family:verdana,sans-serif">Create a new class in XamFormsWpf.WPF called HelloService with the following code</span></b><br>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Services;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.WPF
{
    <span style="color:rgb(51,51,51);font-weight:bold">class</span> HelloService : IHelloService
    {
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> <span style="color:rgb(0,134,179)">string</span> SayHello()
        {
            <span style="color:rgb(51,51,51);font-weight:bold">return</span> <span style="color:rgb(221,17,68)">"Hello, Windows Desktop!"</span>;
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFhhbUZvcm1zV3BmLkNvcmUuU2VydmljZXM7PGJyPjxicj5uYW1lc3Bh Y2UgWGFtRm9ybXNXcGYuV1BGLlNlcnZpY2VzPGJyPns8YnI+wqDCoMKgIGNsYXNzIEhlbGxvU2Vy dmljZSA6IElIZWxsb1NlcnZpY2U8YnI+wqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqAgcHVibGlj IHN0cmluZyBTYXlIZWxsbygpPGJyPsKgwqDCoMKgwqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqDC oMKgwqDCoCByZXR1cm4gIkhlbGxvLCBXaW5kb3dzIERlc2t0b3AhIjs8YnI+wqDCoMKgwqDCoMKg wqAgfTxicj7CoMKgwqAgfTxicj59PGJyPgpgYGA=">​</div>
</div>
<b><span style="font-family:verdana,sans-serif">In XamFormsWpf.WPF, update the App.xaml.cs file with the following code.</span></b><br>
</div>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> System.Windows;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Services;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.WPF
{
    <span style="color:rgb(153,153,136);font-style:italic">/// &lt;summary&gt;</span>
    <span style="color:rgb(153,153,136);font-style:italic">/// Interaction logic for App.xaml</span>
    <span style="color:rgb(153,153,136);font-style:italic">/// &lt;/summary&gt;</span>
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> partial <span style="color:rgb(51,51,51);font-weight:bold">class</span> App : Application
    {
        SimpleIoC IoC = SimpleIoC.Container;
        <span style="color:rgb(51,51,51);font-weight:bold">protected</span> override <span style="color:rgb(51,51,51);font-weight:bold">void</span> OnStartup(StartupEventArgs e)
        {
            IoC.Register&lt;IHelloService, HelloService&gt;();
        }
    }
}
</code></pre>
<hr>
<h2>Creating some simple data binding in MainPage and MainWIndow<br>
</h2>
<h3><a name="TOC-Overview2"></a><span style="font-family:verdana,sans-serif">Overview</span></h3>
<span style="font-family:verdana,sans-serif"><b>The final step is to make use of our ContactViewModel in our Forms and and WPF XAML views.&nbsp; The Xamarin.Forms and WPF XAML definitions are not interchangeable, meaning we will need to use the different view definitions on each platform, but with our project structure, we can reuse all of our Core PCL content.<br>
<br>
</b></span></div>
<div><span style="font-family:verdana,sans-serif"><b>First we will update our MainPage.xaml file in our Forms PCL, then we'll move on to provide the same implementation in our WPF project.</b></span></div>
<div>
<h3><span style="font-family:verdana,sans-serif">Steps</span></h3>
<p><b><span style="font-family:verdana,sans-serif">Open the MainPage.xaml file in your XamFormsWpf.Core.Forms PCL and add the following code.</span></b><br>
</p>
</div>
<div>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(153,153,153);font-weight:bold">&lt;?xml version="1.0" encoding="utf-8" ?&gt;</span>
<span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">ContentPage</span> <span style="color:rgb(0,128,128)">xmlns</span>=<span style="color:rgb(221,17,68)">"http://xamarin.com/schemas/2014/forms"</span>
             <span style="color:rgb(0,128,128)">xmlns:x</span>=<span style="color:rgb(221,17,68)">"http://schemas.microsoft.com/winfx/2009/xaml"</span>
             <span style="color:rgb(0,128,128)">xmlns:local</span>=<span style="color:rgb(221,17,68)">"clr-namespace:XamFormsWpf"</span>
             <span style="color:rgb(0,128,128)">x:Class</span>=<span style="color:rgb(221,17,68)">"XamFormsWpf.MainPage"</span>&gt;</span>
  <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span>&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Label</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding Hello}"</span> /&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span> <span style="color:rgb(0,128,128)">Orientation</span>=<span style="color:rgb(221,17,68)">"Horizontal"</span>&gt;</span>
      <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Label</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"First Name"</span> /&gt;</span>
      <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Entry</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding ContactFirstName, Mode=TwoWay}"</span> /&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span>&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span> <span style="color:rgb(0,128,128)">Orientation</span>=<span style="color:rgb(221,17,68)">"Horizontal"</span>&gt;</span>
      <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Label</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"Last Name"</span> /&gt;</span>
      <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Entry</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding ContactLastName, Mode=TwoWay}"</span> /&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span>&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span> <span style="color:rgb(0,128,128)">Orientation</span>=<span style="color:rgb(221,17,68)">"Horizontal"</span>&gt;</span>
      <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Label</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"Email"</span> /&gt;</span>
      <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Entry</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding ContactEmail, Mode=TwoWay}"</span> /&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span>&gt;</span>
  <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackLayout</span>&gt;</span>
<span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">ContentPage</span>&gt;</span>
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgWE1MPGJyPiZsdDs/eG1sIHZlcnNpb249IjEuMCIgZW5jb2Rpbmc9InV0Zi04IiA/Jmd0Ozxi cj4mbHQ7Q29udGVudFBhZ2UgeG1sbnM9Imh0dHA6Ly94YW1hcmluLmNvbS9zY2hlbWFzLzIwMTQv Zm9ybXMiPGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCB4bWxuczp4PSJodHRwOi8vc2NoZW1h cy5taWNyb3NvZnQuY29tL3dpbmZ4LzIwMDkveGFtbCI8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDC oMKgIHhtbG5zOmxvY2FsPSJjbHItbmFtZXNwYWNlOlhhbUZvcm1zV3BmIjxicj7CoMKgwqDCoMKg wqDCoMKgwqDCoMKgwqAgeDpDbGFzcz0iWGFtRm9ybXNXcGYuTWFpblBhZ2UiJmd0Ozxicj7CoCAm bHQ7U3RhY2tMYXlvdXQmZ3Q7PGJyPsKgwqDCoCAmbHQ7TGFiZWwgVGV4dD0ie0JpbmRpbmcgSGVs bG99IiAvJmd0Ozxicj7CoMKgwqAgJmx0O1N0YWNrTGF5b3V0IE9yaWVudGF0aW9uPSJIb3Jpem9u dGFsIiZndDs8YnI+wqDCoMKgwqDCoCAmbHQ7TGFiZWwgVGV4dD0iRmlyc3QgTmFtZSIgLyZndDs8 YnI+wqDCoMKgwqDCoCAmbHQ7RW50cnkgVGV4dD0ie0JpbmRpbmcgQ29udGFjdEZpcnN0TmFtZSwg TW9kZT1Ud29XYXl9IiAvJmd0Ozxicj7CoMKgwqAgJmx0Oy9TdGFja0xheW91dCZndDs8YnI+wqDC oMKgICZsdDtTdGFja0xheW91dCBPcmllbnRhdGlvbj0iSG9yaXpvbnRhbCImZ3Q7PGJyPsKgwqDC oMKgwqAgJmx0O0xhYmVsIFRleHQ9Ikxhc3QgTmFtZSIgLyZndDs8YnI+wqDCoMKgwqDCoCAmbHQ7 RW50cnkgVGV4dD0ie0JpbmRpbmcgQ29udGFjdExhc3ROYW1lLCBNb2RlPVR3b1dheX0iIC8mZ3Q7 PGJyPsKgwqDCoCAmbHQ7L1N0YWNrTGF5b3V0Jmd0Ozxicj7CoMKgwqAgJmx0O1N0YWNrTGF5b3V0 IE9yaWVudGF0aW9uPSJIb3Jpem9udGFsIiZndDs8YnI+wqDCoMKgwqDCoCAmbHQ7TGFiZWwgVGV4 dD0iRW1haWwiIC8mZ3Q7PGJyPsKgwqDCoMKgwqAgJmx0O0VudHJ5IFRleHQ9IntCaW5kaW5nIENv bnRhY3RFbWFpbCwgTW9kZT1Ud29XYXl9IiAvJmd0Ozxicj7CoMKgwqAgJmx0Oy9TdGFja0xheW91 dCZndDs8YnI+wqAgJmx0Oy9TdGFja0xheW91dCZndDs8YnI+Jmx0Oy9Db250ZW50UGFnZSZndDs8 YnI+CmBgYA==">​</div>
</div>
<b><span style="font-family:verdana,sans-serif">Now open the MainPage.xaml.cs code-behind file and add the following code.&nbsp; This will setup some simple controls, a sample Contact, and demonstrate data binding between the two.</span></b><br>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> Xamarin.Forms;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Data;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.ViewModels;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf
{
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> partial <span style="color:rgb(51,51,51);font-weight:bold">class</span> MainPage : ContentPage
    {
        readonly ContactViewModel _contactViewModel;
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> MainPage()
        {
            <span style="color:rgb(153,153,136);font-style:italic">// Test contact</span>
            var contact = <span style="color:rgb(51,51,51);font-weight:bold">new</span> Contact {
                FirstName = <span style="color:rgb(221,17,68)">"Jimmy"</span>,
                LastName = <span style="color:rgb(221,17,68)">"Smith"</span>,
                Email = <span style="color:rgb(221,17,68)">"Jimmy.Smith@gmail.com"</span>
            };

            _contactViewModel = <span style="color:rgb(51,51,51);font-weight:bold">new</span> ContactViewModel(contact);
            BindingContext = _contactViewModel;
            InitializeComponent();
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFhhbWFyaW4uRm9ybXM7PGJyPnVzaW5nIFhhbUZvcm1zV3BmLkNvcmUu RGF0YTs8YnI+dXNpbmcgWGFtRm9ybXNXcGYuQ29yZS5WaWV3TW9kZWxzOzxicj48YnI+bmFtZXNw YWNlIFhhbUZvcm1zV3BmPGJyPns8YnI+wqDCoMKgIHB1YmxpYyBwYXJ0aWFsIGNsYXNzIE1haW5Q YWdlIDogQ29udGVudFBhZ2U8YnI+wqDCoMKgIHs8YnI+wqDCoMKgwqDCoMKgwqAgcmVhZG9ubHkg Q29udGFjdFZpZXdNb2RlbCBfY29udGFjdFZpZXdNb2RlbDs8YnI+wqDCoMKgwqDCoMKgwqAgcHVi bGljIE1haW5QYWdlKCk8YnI+wqDCoMKgwqDCoMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDC oMKgIC8vIFRlc3QgY29udGFjdDxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIHZhciBjb250YWN0 ID0gbmV3IENvbnRhY3Qgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgRmlyc3RO YW1lID0gIkppbW15Iiw8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgIExhc3ROYW1l ID0gIlNtaXRoIiw8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgIEVtYWlsID0gIkpp bW15LlNtaXRoQGdtYWlsLmNvbSI8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCB9Ozxicj48YnI+ wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBfY29udGFjdFZpZXdNb2RlbCA9IG5ldyBDb250YWN0Vmll d01vZGVsKGNvbnRhY3QpOzxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgIEJpbmRpbmdDb250ZXh0 ID0gX2NvbnRhY3RWaWV3TW9kZWw7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgSW5pdGlhbGl6 ZUNvbXBvbmVudCgpOzxicj7CoMKgwqDCoMKgwqDCoCB9PGJyPsKgwqDCoCB9PGJyPn08YnI+CmBg YA==">​</div>
</div>
<b><span style="font-family:verdana,sans-serif">The final step is to implement similar view controls in the WPF project.&nbsp; In XamFormsWpf.WPF, open MainWindow.xaml and add the following code</span></b><br>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Window</span> <span style="color:rgb(0,128,128)">x:Class</span>=<span style="color:rgb(221,17,68)">"XamFormsWpf.WPF.MainWindow"</span>
        <span style="color:rgb(0,128,128)">xmlns</span>=<span style="color:rgb(221,17,68)">"http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
        <span style="color:rgb(0,128,128)">xmlns:x</span>=<span style="color:rgb(221,17,68)">"http://schemas.microsoft.com/winfx/2006/xaml"</span>
        <span style="color:rgb(0,128,128)">xmlns:d</span>=<span style="color:rgb(221,17,68)">"http://schemas.microsoft.com/expression/blend/2008"</span>
        <span style="color:rgb(0,128,128)">xmlns:mc</span>=<span style="color:rgb(221,17,68)">"http://schemas.openxmlformats.org/markup-compatibility/2006"</span>
        <span style="color:rgb(0,128,128)">xmlns:local</span>=<span style="color:rgb(221,17,68)">"clr-namespace:XamFormsWpf.WPF"</span>
        <span style="color:rgb(0,128,128)">mc:Ignorable</span>=<span style="color:rgb(221,17,68)">"d"</span>
        <span style="color:rgb(0,128,128)">Title</span>=<span style="color:rgb(221,17,68)">"MainWindow"</span> <span style="color:rgb(0,128,128)">Height</span>=<span style="color:rgb(221,17,68)">"350"</span> <span style="color:rgb(0,128,128)">Width</span>=<span style="color:rgb(221,17,68)">"525"</span>&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Grid</span>&gt;</span>
        <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span>&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBlock</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding Hello}"</span> /&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span> <span style="color:rgb(0,128,128)">Orientation</span>=<span style="color:rgb(221,17,68)">"Horizontal"</span>&gt;</span>
                <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBlock</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"First Name"</span> /&gt;</span>
                <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBox</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding ContactFirstName, Mode=TwoWay}"</span> /&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span>&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span> <span style="color:rgb(0,128,128)">Orientation</span>=<span style="color:rgb(221,17,68)">"Horizontal"</span>&gt;</span>
                <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBlock</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"Last Name"</span> /&gt;</span>
                <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBox</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding ContactLastName, Mode=TwoWay}"</span> /&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span>&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span> <span style="color:rgb(0,128,128)">Orientation</span>=<span style="color:rgb(221,17,68)">"Horizontal"</span>&gt;</span>
                <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBlock</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"Email"</span> /&gt;</span>
                <span style="color:rgb(0,0,128);font-weight:normal">&lt;<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">TextBox</span> <span style="color:rgb(0,128,128)">Text</span>=<span style="color:rgb(221,17,68)">"{Binding ContactEmail, Mode=TwoWay}"</span> /&gt;</span>
            <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span>&gt;</span>
        <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">StackPanel</span>&gt;</span>
    <span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Grid</span>&gt;</span>
<span style="color:rgb(0,0,128);font-weight:normal">&lt;/<span style="color:rgb(153,0,0);font-weight:bold;color:rgb(0,0,128);font-weight:normal">Window</span>&gt;</span>
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgWE1MPGJyPiZsdDtXaW5kb3cgeDpDbGFzcz0iWGFtRm9ybXNXcGYuV1BGLk1haW5XaW5kb3ci PGJyPsKgwqDCoMKgwqDCoMKgIHhtbG5zPSJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dp bmZ4LzIwMDYveGFtbC9wcmVzZW50YXRpb24iPGJyPsKgwqDCoMKgwqDCoMKgIHhtbG5zOng9Imh0 dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd2luZngvMjAwNi94YW1sIjxicj7CoMKgwqDCoMKg wqDCoCB4bWxuczpkPSJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL2V4cHJlc3Npb24vYmxl bmQvMjAwOCI8YnI+wqDCoMKgwqDCoMKgwqAgeG1sbnM6bWM9Imh0dHA6Ly9zY2hlbWFzLm9wZW54 bWxmb3JtYXRzLm9yZy9tYXJrdXAtY29tcGF0aWJpbGl0eS8yMDA2Ijxicj7CoMKgwqDCoMKgwqDC oCB4bWxuczpsb2NhbD0iY2xyLW5hbWVzcGFjZTpYYW1Gb3Jtc1dwZi5XUEYiPGJyPsKgwqDCoMKg wqDCoMKgIG1jOklnbm9yYWJsZT0iZCI8YnI+wqDCoMKgwqDCoMKgwqAgVGl0bGU9Ik1haW5XaW5k b3ciIEhlaWdodD0iMzUwIiBXaWR0aD0iNTI1IiZndDs8YnI+wqDCoMKgICZsdDtHcmlkJmd0Ozxi cj7CoMKgwqDCoMKgwqDCoCAmbHQ7U3RhY2tQYW5lbCZndDs8YnI+wqDCoMKgwqDCoMKgwqDCoMKg wqDCoCAmbHQ7VGV4dEJsb2NrIFRleHQ9IntCaW5kaW5nIEhlbGxvfSIgLyZndDs8YnI+wqDCoMKg wqDCoMKgwqDCoMKgwqDCoCAmbHQ7U3RhY2tQYW5lbCBPcmllbnRhdGlvbj0iSG9yaXpvbnRhbCIm Z3Q7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCAmbHQ7VGV4dEJsb2NrIFRleHQ9 IkZpcnN0IE5hbWUiIC8mZ3Q7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCAmbHQ7 VGV4dEJveCBUZXh0PSJ7QmluZGluZyBDb250YWN0Rmlyc3ROYW1lLCBNb2RlPVR3b1dheX0iIC8m Z3Q7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgJmx0Oy9TdGFja1BhbmVsJmd0Ozxicj7CoMKg wqDCoMKgwqDCoMKgwqDCoMKgICZsdDtTdGFja1BhbmVsIE9yaWVudGF0aW9uPSJIb3Jpem9udGFs IiZndDs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgICZsdDtUZXh0QmxvY2sgVGV4 dD0iTGFzdCBOYW1lIiAvJmd0Ozxicj7CoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgJmx0 O1RleHRCb3ggVGV4dD0ie0JpbmRpbmcgQ29udGFjdExhc3ROYW1lLCBNb2RlPVR3b1dheX0iIC8m Z3Q7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqAgJmx0Oy9TdGFja1BhbmVsJmd0Ozxicj7CoMKg wqDCoMKgwqDCoMKgwqDCoMKgICZsdDtTdGFja1BhbmVsIE9yaWVudGF0aW9uPSJIb3Jpem9udGFs IiZndDs8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgICZsdDtUZXh0QmxvY2sgVGV4 dD0iRW1haWwiIC8mZ3Q7PGJyPsKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKgwqDCoCAmbHQ7VGV4 dEJveCBUZXh0PSJ7QmluZGluZyBDb250YWN0RW1haWwsIE1vZGU9VHdvV2F5fSIgLyZndDs8YnI+ wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCAmbHQ7L1N0YWNrUGFuZWwmZ3Q7PGJyPsKgwqDCoMKgwqDC oMKgICZsdDsvU3RhY2tQYW5lbCZndDs8YnI+wqDCoMKgICZsdDsvR3JpZCZndDs8YnI+Jmx0Oy9X aW5kb3cmZ3Q7PGJyPmBgYA==">​</div>
</div>
<br>
<b><span style="font-family:verdana,sans-serif">Now open the MainWindow.xaml.cs code-behind file and add the following 
code.&nbsp; This will setup some simple controls, a sample Contact, and 
demonstrate data binding between the two.</span></b><br>
<div>
<pre style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;font-size:1em;line-height:1.2em;margin:1.2em 0px"><code style="font-size:0.85em;font-family:Consolas,Inconsolata,Courier,monospace;margin:0px 0.15em;padding:0px 0.3em;white-space:pre-wrap;border:1px solid rgb(234,234,234);background-color:rgb(248,248,248);border-radius:3px;display:inline;white-space:pre;overflow:auto;border-radius:3px;border:1px solid rgb(204,204,204);padding:0.5em 0.7em;display:block!important;display:block;overflow-x:auto;padding:0.5em;color:rgb(51,51,51);background:rgb(248,248,248) none repeat scroll 0% 0%"><span style="color:rgb(51,51,51);font-weight:bold">using</span> System.Windows;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.Data;
<span style="color:rgb(51,51,51);font-weight:bold">using</span> XamFormsWpf.Core.ViewModels;

<span style="color:rgb(51,51,51);font-weight:bold">namespace</span> XamFormsWpf.WPF
{
    <span style="color:rgb(153,153,136);font-style:italic">/// &lt;summary&gt;</span>
    <span style="color:rgb(153,153,136);font-style:italic">/// Interaction logic for MainWindow.xaml</span>
    <span style="color:rgb(153,153,136);font-style:italic">/// &lt;/summary&gt;</span>
    <span style="color:rgb(51,51,51);font-weight:bold">public</span> partial <span style="color:rgb(51,51,51);font-weight:bold">class</span> MainWindow : Window
    {
        readonly ContactViewModel _contactViewModel;
        <span style="color:rgb(51,51,51);font-weight:bold">public</span> MainWindow()
        {
            var contact = <span style="color:rgb(51,51,51);font-weight:bold">new</span> Contact {
                FirstName = <span style="color:rgb(221,17,68)">"Jimmy"</span>,
                LastName = <span style="color:rgb(221,17,68)">"Smith"</span>,
                Email = <span style="color:rgb(221,17,68)">"Jimmy.Smith@gmail.com"</span>
            };

            _contactViewModel = <span style="color:rgb(51,51,51);font-weight:bold">new</span> ContactViewModel(contact);
            DataContext = _contactViewModel;

            InitializeComponent();
        }
    }
}
</code></pre>
<div style="height:0;width:0;max-height:0;max-width:0;overflow:hidden;font-size:0em;padding:0;margin:0" title="MDH:YGBgQysrPGJyPnVzaW5nIFN5c3RlbS5XaW5kb3dzOzxicj51c2luZyBYYW1Gb3Jtc1dwZi5Db3Jl LkRhdGE7PGJyPnVzaW5nIFhhbUZvcm1zV3BmLkNvcmUuVmlld01vZGVsczs8YnI+PGJyPm5hbWVz cGFjZSBYYW1Gb3Jtc1dwZi5XUEY8YnI+ezxicj7CoMKgwqAgLy8vICZsdDtzdW1tYXJ5Jmd0Ozxi cj7CoMKgwqAgLy8vIEludGVyYWN0aW9uIGxvZ2ljIGZvciBNYWluV2luZG93LnhhbWw8YnI+wqDC oMKgIC8vLyAmbHQ7L3N1bW1hcnkmZ3Q7PGJyPsKgwqDCoCBwdWJsaWMgcGFydGlhbCBjbGFzcyBN YWluV2luZG93IDogV2luZG93PGJyPsKgwqDCoCB7PGJyPsKgwqDCoMKgwqDCoMKgIHJlYWRvbmx5 IENvbnRhY3RWaWV3TW9kZWwgX2NvbnRhY3RWaWV3TW9kZWw7PGJyPsKgwqDCoMKgwqDCoMKgIHB1 YmxpYyBNYWluV2luZG93KCk8YnI+wqDCoMKgwqDCoMKgwqAgezxicj7CoMKgwqDCoMKgwqDCoMKg wqDCoMKgIHZhciBjb250YWN0ID0gbmV3IENvbnRhY3Qgezxicj7CoMKgwqDCoMKgwqDCoMKgwqDC oMKgwqDCoMKgwqAgRmlyc3ROYW1lID0gIkppbW15Iiw8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDC oMKgwqDCoMKgIExhc3ROYW1lID0gIlNtaXRoIiw8YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoMKg wqDCoMKgIEVtYWlsID0gIkppbW15LlNtaXRoQGdtYWlsLmNvbSI8YnI+wqDCoMKgwqDCoMKgwqDC oMKgwqDCoCB9Ozxicj48YnI+wqDCoMKgwqDCoMKgwqDCoMKgwqDCoCBfY29udGFjdFZpZXdNb2Rl bCA9IG5ldyBDb250YWN0Vmlld01vZGVsKGNvbnRhY3QpOzxicj7CoMKgwqDCoMKgwqDCoMKgwqDC oMKgIERhdGFDb250ZXh0ID0gX2NvbnRhY3RWaWV3TW9kZWw7PGJyPjxicj7CoMKgwqDCoMKgwqDC oMKgwqDCoMKgIEluaXRpYWxpemVDb21wb25lbnQoKTs8YnI+wqDCoMKgwqDCoMKgwqAgfTxicj7C oMKgwqAgfTxicj59PGJyPgpgYGA=">​</div>
</div>
<br>
<hr>
<h2>Conclusion</h2>
<b><span style="font-family:verdana,sans-serif">That's all there is to it! You can now build and run the app by selecting a project as the default startup project.</span></b><b><span style="font-family:verdana,sans-serif">&nbsp; Below are screenshots of the application running on Android and (of course) WPF.<br>
<br>
</span></b></div>
<div><b><span style="font-family:verdana,sans-serif">Android:<br>
<div style="display:block;text-align:left"><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/08_DroidScreenshot.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/08_DroidScreenshot.PNG" height="400" border="0" width="206"></a><br>
<br>
</div>
WPF:<br>
</span></b></div>
<div><b><span style="font-family:verdana,sans-serif">
<div style="display:block;text-align:left"><a href="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/09_WPFScreenshot.PNG?attredirects=0" imageanchor="1"><img src="https://sites.google.com/site/netdeveloperblog/xamarin/forms/xamarin-forms-and-wpf/09_WPFScreenshot.PNG" border="0"></a></div>
</span></b></div>
<div>
<div><b><span style="font-family:verdana,sans-serif"><br>
To summarize, in this tutorial, we've gone through the steps necessary to add support for a WPF project to a Xamarin.Forms solution.&nbsp; We also created a very simple implementation as an example.&nbsp; We covered the following topics:<br>
</span></b></div>
<div>
<ul><li><span style="font-family:verdana,sans-serif"><b>Creating a new solution from the default Xamarin.Forms project template</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Adding a Core PCL to our solution, to contain our common code</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Add some common code to our Core PCL<br>
</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Using a simple IoC container to replace Xamarin.Forms DependencyService class</b></span></li>
<li><span style="font-family:verdana,sans-serif"><b>Add a WPF project to our solution to target Windows Desktop</b></span></li>
<li><b><span style="font-family:verdana,sans-serif">Implementing IHelloService in each platform</span></b><br>
</li>
<li><span style="font-family:verdana,sans-serif"><b>Creating some simple data binding in our MainPage and MainWindow views</b></span></li></ul>
</div>
<div><br>
</div>
<font face="verdana,sans-serif"><b>As usual</b></font><b><font face="verdana,sans-serif">,
 styling was omitted from this tutorial, but now that you understand these
 core concepts, you can work with the existing solution to add some 
styling of your own.<br>
</font></b></div>
<div>
<div><br>
<b><span style="font-family:verdana,sans-serif">I truly hope you found this tutorial to be both clear and informative, and please feel free to leave feedback.</span></b><br>
<br>
</div>
</div>
<div></div>
