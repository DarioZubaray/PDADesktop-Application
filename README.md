[![](https://www.imagosur.com.ar/images/imago-sur-logo.png)](https://www.imagosur.com.ar/es/)

# PDADesktop

### Herramientas
##### [Squirrel.window](https://github.com/Squirrel/Squirrel.Windows) - Installer and updater manager
  - Instalación

```xml
PM> Install-Package Squirrel.Windows
```

###### [Visual Studio Build Packaging](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/visual-studio-packaging.md)
  - El proyecto cuenta con 2 tareas de compilación ([MSBuild Targets](https://msdn.microsoft.com/en-us/library/ms171462.aspx)) para _Debug_ y _Release_:

```xml
<!-- Remove obj folder -->
<RemoveDir Directories="$(BaseIntermediateOutputPath)" />
<!-- Remove bin folder -->
<RemoveDir Directories="$(BaseOutputPath)" />
...
<Copy SourceFiles="..\packages\squirrel.windows.1.8.0\tools\Squirrel.exe" DestinationFiles="$(OutputPath)\..\Update.exe" />
```

  - Sólo para el target **Release** existe el empaquetado con nuget y generación con squirrel

```xml
<Exec Command="nuget pack PDADesktop.nuspec -Version %(myAssemblyInfo.Version) -Properties Configuration=Release -OutputDirectory .\bin\Release" />

<Exec Command="squirrel --releasify $(OutDir)PDADesktop.$([System.Version]::Parse(%(myAssemblyInfo.Version)).ToString(3)).nupkg" />
```

  - Nota 1: ```nuget.exe``` podria ser una variable de entorno para facilitar las cosas o se puede instalar el paquete ```NuGet.CommandLine``` en la solucion.

```
PM>  Install-Package NuGet.CommandLine
```

  - Nota 2: Para el error ```Exec Command call. 'squirrel' is not recognized as an internal or external command``` también se podria tener ```Squirrel.exe``` en la variable de entorno _path_ o agregar la ruta completa al mismo ejecutable en el MSBuild targer. Para la aplicacion cree un variable de entorno apuntando a ```C:\dev\squirrel\``` donde copie ```TODO``` el contendio de ```<Proyecto>\packages\squirrel.windows.1.8.0\tools``` porque al parecer ```Squirrel.exe``` hace uso de ```StubFile.exe```


##### Recurso éstatico JBOSS
Squirrel necesita verificar en un directorio (carpeta local o url) los propios archivos de instalción o actualización:
>aplicacion-version-delta.nupkg
>aplicacion-version-full.nupkg
>RELEASES
>Setup.exe
>Setup.msi

1. Habilitar el parametro de listar carpetar en web.xml de ```jboss-4.2.3.GA/server/<instancia>/deploy/jboss-web.deployer/conf/web.xml```


```xml
    <servlet>
        <servlet-name>default</servlet-name>
        <servlet-class>org.apache.catalina.servlets.DefaultServlet</servlet-class>
        <init-param>
            <param-name>debug</param-name>
            <param-value>0</param-value>
        </init-param>
        <init-param>
            <param-name>listings</param-name>
			<!-- Habilitar el parametro listing -->
            <param-value>true</param-value>
        </init-param>
        <load-on-startup>1</load-on-startup>
    </servlet>
```

2. Modificar el server.xml de jboss-web ```jboss-4.2.3.GA/server/<instancia>/deploy/jboss-web.deployer/server.xml``` agregando la etiqueta content con la ruta de la carpeta y el path con el que accedemos:

```xml
	<Host name="localhost"
           autoDeploy="false" deployOnStartup="false" deployXML="false"
           configClass="org.jboss.web.tomcat.security.config.JBossContextConfig">
		<!-- Agregar el Context con la direccion en disco y la ruta  -->
		<Context docBase="${jboss.home.dir}/static/PDADesktop/Release" path='/static/PDADesktop/Release' ></Context>
	</Host>	
```

3. De contar con las carpetas creada en ```jboss-4.2.3.GA/static/PDADesktop/Release``` y con los archivos necesarios accedemos 

```
http://localhost:8080/static/PDADesktop/Release/
```

_NOTA: a veces se cachean los archivos, no se actualiza la lista, estos quedan guardados en la carpeta *work* porque eliminar a menudo ayudará_


##### [Log4net](https://logging.apache.org/log4net/)

  - Basado en la guia de [stackify.com](https://stackify.com/log4net-guide-dotnet-logging/), el comando para instalar es:

```
PM> Install-Package log4net
```

  - Necesita contar con el archivo ```log4net.config```

```xml
<log4net>
    <root>
      <level value="ALL" />
      <appender-ref ref="console" />
      <appender-ref ref="file" />
    </root>
    <appender name="console" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="file" type="log4net.Appender.RollingFileAppender">
      <file value="pdaDesktop.log" />
      <appendToFile value="true" />
      <rollingStyle value="Size" />
      <maxSizeRollBackups value="5" />
      <maximumFileSize value="10MB" />
      <staticLogFileName value="true" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %level %logger - %message%newline" />
      </layout>
    </appender>
  </log4net>
```

  -Nota: muy **muy** importante, es copiar el archivo siempre a la carpeta de salida, en la ventana de propiedades > copiar en el directorio de salida: 'Copiar Siempre' (Lo cual modifica el Proyecto.csproj):

```xml
<None Include="log4net.config">
  <CopyToOutputDirectory>Always</CopyToOutputDirectory>
</None>
```

  - Luego hay que declararlo en el ```assembly.info```

```
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
```

  - La forma de declararlo como un atributo de la clase

```CSHARP
private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
```

##### [Material Design in xaml](http://materialdesigninxaml.net/)
  - Nuget pack

```
PM> Install-Package MaterialDesignThemes
```

  - Incluye lo siguiente en ```App.xaml```

```xml
<Application . . . >
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml" />
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml" />
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Amber.xaml" />
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Amber.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

  - En los archivos ```*.xaml``` se incluyen las directivas en el tag de ```Window```

```xml
 <Window . . .
        xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"
        TextElement.Foreground="{DynamicResource MaterialDesignBody}"
        TextElement.FontWeight="Regular"
        TextElement.FontSize="13"
        TextOptions.TextFormattingMode="Ideal" 
        TextOptions.TextRenderingMode="Auto"        
        Background="{DynamicResource MaterialDesignPaper}"
        FontFamily="{DynamicResource MaterialDesignFont}">
    ...
 ```
 
##### [MahApps.Metro](https://mahapps.com/)

 - MahApps tiene su propio paquete nuget (_no va a ser usado en la aplicación final_)

```
 PM> Install-Package MahApps.Metro
```

   - Integracion con Material Design

```
 PM> Install-Package MaterialDesignThemes.MahApps
```

   - Hay que mergear los diccionarioss en el App.xaml (Hay que elegir entre los temas **BaseLight** or **BaseDark**, la guia dice: 
   >el color de acento va a ser sobreescrito por Material Design's después, asi que no tiene mucha importacia)

   - Pero el color de la ventana y demas,  se da con el color ```component/Styles/Accents/Amber.xaml``` por defecto es azul.

```xml
 <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml" />
<ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml" />
<ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml" />
<ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Accents/Amber.xaml" />
<ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml" />
```

   - Hay que cambiar la etiqueta ```windows``` por ```MetroWindows```

```xml
<metro:MetroWindow [...]
                   xmlns:metro="http://metro.mahapps.com/winfx/xaml/controls"
                   GlowBrush="{DynamicResource AccentColorBrush}"
                   BorderThickness="1"
                   [...]>
```

  - Adicionalmente será necesario extender la clase por detrás a la superclase ```MetroWindow```

```CSHARP
using MahApps.Metro.Controls;

public partial class MainWindow : MetroWindow
```

##### XDT (XML-Document-Transform)
  - La aplicacion cuenta con 2 configuraciones para ser ejecutados en cada target: *Debug* y *Release*, ademas de los evidentes archivos ```App.Debug.config``` y ```App.Release.config```, en el csproj se agrega el siguiente msBuild target:

```xml
<UsingTask TaskName="TransformXml" AssemblyFile="$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)\Web\Microsoft.Web.Publishing.Tasks.dll" />
  <Target Name="App_config_AfterCompile" AfterTargets="AfterCompile" Condition="Exists('App.$(Configuration).config')">
    <!--Generate transformed app config in the intermediate directory-->
    <TransformXml Source="App.config" Destination="$(IntermediateOutputPath)$(TargetFileName).config" Transform="App.$(Configuration).config" />
    <!--Force build process to use the transformed configuration file from now on.-->
    <ItemGroup>
      <AppConfigWithTargetPath Remove="App.config" />
      <AppConfigWithTargetPath Include="$(IntermediateOutputPath)$(TargetFileName).config">
        <TargetPath>$(TargetFileName).config</TargetPath>
      </AppConfigWithTargetPath>
    </ItemGroup>
  </Target>
  <!--Override After Publish to support ClickOnce AfterPublish. Target replaces the untransformed config file copied to the deployment directory with the transformed one.-->
  <Target Name="App_config_AfterPublish" AfterTargets="AfterPublish" Condition="Exists('App.$(Configuration).config')">
    <PropertyGroup>
      <DeployedConfig>$(_DeploymentApplicationDir)$(TargetName)$(TargetExt).config$(_DeploymentFileMappingExtension)</DeployedConfig>
    </PropertyGroup>
    <!--Publish copies the untransformed App.config to deployment directory so overwrite it-->
    <Copy Condition="Exists('$(DeployedConfig)')" SourceFiles="$(IntermediateOutputPath)$(TargetFileName).config" DestinationFiles="$(DeployedConfig)" />
  </Target>
```

#### [Microsoft Expression](https://www.nuget.org/packages/Expression.Blend.Sdk/)
Contiene ```System.Windows.Interactivity``` para:

 - WPF 4.0, 4.5
 - Silverligt 4.0, 5.0
 - Windows Phone 7.1, 8.0
 - Windows Store 8, 8.1

```
PM> Install-Package Expression.Blend.Sdk -Version 1.0.2
```


##### IoC/DI container
#### [StructureMap](http://structuremap.github.io/)

El ```App.xaml.cs``` tiene una atributo ```StructureMap.Container``` y en el mismo constructor de la clase lo inicializa pasando una nueva instancia de la clase de configuracion ```MyContainerInitializer``` en la cual se define
```CSHARP
    Scan(scanner =>
    {
        scanner.TheCallingAssembly();
        scanner.WithDefaultConventions();
    });
```
Además se definen ```ForConcreteType``` de cada dependencia a injectar. Luego se van obteniendo de la forma
```CSHARP
this.Container.GetInstance<MainWindowViewModel>()
```


#### [Alert Bar for WPF](https://github.com/chadkuehn/AlertBarWpf)
- Instalación:
```
PM> Install-Package AlertBarWpf
```
 - declaracion del namespace:
```xml
<Window ...
    xmlns:mbar="clr-namespace:AlertBarWpf;assembly=AlertBarWpf">
```
 - Se crea un control:
```xml
<mbar:AlertBarWpf x:Name="msgbar" />
```
 - En el behind code:
```CSHARP
msgbar.Clear();
msgbar.SetDangerAlert("Select an Item.");
```
 - API:
    - ```SetDangerAlert```
    - ```SetSuccessAlert```
    - ```SetWarningAlert```
    - ```SetInformationAlert```
 - Sobrecargas:
   - ```Set*Alert(string message, int timeoutInSeconds = 0) ``` 

#### [Microsoft.Net.Http](https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client)

  - Intalación

```
PM> Install-Package Microsoft.Net.Http
```

#### [Newtonsoft.Json](https://www.newtonsoft.com/json)
  
  - Intalación

```
PM> Install-Package Newtonsoft.Json
```

 - Serializacion

```CSHARP
Product product = new Product();
product.Name = "Apple";
product.Expiry = new DateTime(2008, 12, 28);
product.Sizes = new string[] { "Small" };

string json = JsonConvert.SerializeObject(product);

// {
//   "Name": "Apple",
//   "Expiry": "2008-12-28T00:00:00",
//   "Sizes": [
//     "Small"
//   ]
// }
```

  - Deserializacion

```CSHARP
string json = @"{
  'Name': 'Bad Boys',
  'ReleaseDate': '1995-4-7T00:00:00',
  'Genres': [
    'Action',
    'Comedy'
  ]
}";

Movie m = JsonConvert.DeserializeObject<Movie>(json);

string name = m.Name;
// Bad Boys
```