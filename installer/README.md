Instrucciones para crear instalador (.vdproj) usando Visual Studio Installer Projects

Resumen rápido
1. Instala la extensión "Visual Studio Installer Projects" desde Extensiones > Administrar extensiones.
2. Compila la solución en `Release` y la plataforma adecuada (x86 o x64 según tus DLL nativas).
3. Usa el asistente para crear un nuevo proyecto de instalación y añade la salida principal y la carpeta `tessdata`.

Pasos detallados

1) Instalar extensión
- En Visual Studio: `Extensiones > Administrar extensiones`.
- Buscar "Visual Studio Installer Projects" (Microsoft) e instalar. Reinicia Visual Studio.

2) Preparar binario
- `Build > Configuration > Release`.
- `Build > Build Solution`.
- Verifica `DotNetOCR\bin\Release\` contiene:
  - `DotNetOCR.exe` (y `.config`/.pdb si aplican)
  - carpeta `tessdata\` con `spa.traineddata`
  - DLL nativas de Tesseract (por ejemplo `libtesseract-*.dll`, `liblept*.dll`) cotejadas con la plataforma (x86/x64)

3) Crear proyecto de instalador
- `Archivo > Nuevo > Proyecto`.
- Busca "Setup Project" (o "Setup and Deployment > Visual Studio Installer > Setup Project").
- Nombre: `DotNetOCR.Setup`.

4) Agregar archivos al instalador
- En el proyecto de Setup abre el "File System Editor" (clic derecho > View > File System).
- Selecciona "Application Folder" y añade:
  - `Primary output` de tu proyecto DotNetOCR (clic derecho > Add > Project Output > selecciona "Primary output").
  - Si `tessdata` no está incluida en la salida automáticamente, elige Add > Folder > crear carpeta `tessdata` y Add > File para agregar `spa.traineddata` y cualquier otro .traineddata.
  - Añade las DLL nativas manualmente si no están en la salida.
- (Opcional) Añade un acceso directo en el menú Inicio: haz clic derecho en `Primary output` > Create Shortcut y arrastra al `User's Programs Menu`.

5) Configurar propiedades del instalador
- En las propiedades del Setup: establece la `TargetPlatform` a `x86` o `x64` según el binario y las DLL nativas.
- Establece ProductName, Manufacturer y versión.

6) Construir instalador
- Build > Build Solution en el proyecto de Setup generará un `.msi` y/o `setup.exe` en la carpeta de salida del proyecto de Setup.

Notas importantes
- Asegúrate de que las DLL nativas coincidan con la arquitectura seleccionada en el instalador.
- Incluye instrucción para instalar Microsoft Visual C++ Redistributable si las DLL nativas lo requieren (2015–2022).
- Si prefieres un instalador más controlado, considera usar Inno Setup o WiX en lugar de .vdproj.

Script auxiliar
- Usa `prepare_installer.ps1` en este directorio para copiar los archivos de `bin\Release` a `installer\package` antes de crear el proyecto de Setup. Ver `prepare_installer.ps1`.