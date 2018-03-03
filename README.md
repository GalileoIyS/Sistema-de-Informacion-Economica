## Sistema de Información Económica
---

El **Sistema de Información Económica** es una plataforma de gestión económica para administraciones públicas que permite analizar, crear indicadores, evaluar y transparentar los datos tributarios de una entidad. Esta herramienta aumenta la capacidad de gestión y eficiencia de las administraciones, a la vez que se genera confianza y transparencia hacia el ciudadano. (Ver ejemplo del [Ayuntamiento de Madrid](http://madrid.sielocal.com/gastos/comparativa/areas))

El sistema está compuesto por los siguientes módulos:
* **Módulo de carga y tratamiento de datos**: Permite cargar información (a partir de un excel con formato predeterminado) y el diseño de indicadores económicos. (ver ejemplo de informes en [Sielocal.com](http://www.sielocal.com/Informes.aspx) 
* **Módulo de visualización**: Permite generar visualizaciones y cuadros de mando con indicadores personalizados. 


Este sistema, implementado de forma extensa en una administración supra-territorial y con un estándar de datos tributarios asociados, podría convertirse en un sistema de remisión de información y coordinación entre distintas administraciones públicas como el ya implantado en el Gobierno de Canarias. (Sistema [UNIFICA](https://prezi.com/ojoqxrp2ygdv/unifica-captura/?utm_campaign=share&utm_medium=copy))

Ejemplos de esta herramienta están actualmente implantados como el [Punto Inteligente de Información Económica](http://madrid.sielocal.com/gastos/comparativa/areas) y en la [plataforma DropKeys](http://www.dropkeys.com).
---

### Visión

El objetivo de este proyecto es disponer de una plataforma capaz de colectar información en un formato estandarizado para analizar datos y crear informes económicos y de infraestructuras. 

    El Real Decreto 835/2003, de 27 de junio regula la cooperación económica del Estado Español a las inversiones de las entidades locales. Permite conocer la situación de las infraestructuras y equipamientos de competencia municipal, formando un inventario de ámbito nacional, de carácter censal, con información precisa y sistematizada de los municipios con población inferior a 50.000 habitantes. 

[Unifica](https://prezi.com/ojoqxrp2ygdv/unifica-captura/?utm_campaign=share&utm_medium=copy) es el sistema de remisión de información del gobierno de Canarias. Ha permitido este [Mapa de infraestructuras]().

La visión de este proyecto es poder disponer de una plataforma capaz de publicar información compleja cruzando información de fuentes diferentes como datos económicos o información georefrenciada, para publicar indicadores elaborados como los informes de [Sielocal.com](http://www.sielocal.com/Informes.aspx) y visualizarlos en un Mapa de Infraestructuras que contiene la información de la Encuesta de Infraestructura y Equipamientos Locales (EIEL) en España cuya finalidad es, según establece el Real Decreto 835/2003, de 27 de junio por el que se regula la cooperación económica del Estado a las inversiones de las entidades locales, "conocer la situación de las infraestructuras y equipamientos de competencia municipal, formando un inventario de ámbito nacional, de carácter censal, con información precisa y sistematizada de los municipios con población inferior a 50.000 habitantes"

Actualmente los módulos se encuentran desarrollados e implementados en diferentes sistemas:

* **Módulo de Remisión de información**: Implementado en el sistema de remisión de información del Gobierno de Canarias. (Ver resultados en el [Mapa de Información de Canarias]( https://www.gobiernodecanarias.org/hacienda/unifica/Transparencia/Mapa/Index)) *Más información en [esta presentación](https://prezi.com/ojoqxrp2ygdv/unifica-captura/?utm_campaign=share&utm_medium=copy)

* **Módulo de Analítico**: Utilizado en el servicio de información [Dropkeys](http://www.dropkeys.com).

* **Módulo de análisis y visualización**: Implantado en la [Federación Colombiana de Municipios](http://colombia.sielocal.com/), Gobierno de Canarias o en el [Sistema de Información Económica Local](sielocal.com) (con indicadores económicos de **más de 10 pases de LATAM**). *Más información en [este video](https://www.youtube.com/watch?v=k4tg07G3_aI)

Galileo pone a disposición de la comunidad Open Source el módulo analítico como pieza central del Sistema de Información Económica. Esperamos que su utilización y evolución en el contexto de la información pública y de gobierno abierto, con el fin de fomentar la transparencia y la participación ciudadana.

![Mapa de Infraestructuras generado a partir de la Encuesta de Infraestructura y Equipamientos Locales (EIEL)](https://github.com/GalileoIyS/Sistema-de-Informacion-Economica/blob/master/Mapa%20UNIFICA.png "Ejemplo de Automatización de Publicación de Información Económica compleja")

Ejemplo de la Automatización de formularios de remisión de información: 

![Automatización de Remisión de Información](https://github.com/GalileoIyS/Sistema-de-Informacion-Economica/blob/master/Creacion%20indicadores.png "Ejemplo de Automatización de Remisión de Información")

### Guía de usuario
Ver el [manual de usuario](https://github.com/GalileoIyS/Sistema-de-Informacion-Economica/blob/master/Manual%20de%20usuario.pdf) completo en formato PDF.

### Guía de instalación
---

#### Módulo Analítico
    1. Instalar la versión Community del Visual Studio (2015 o superior): https://www.visualstudio.com/es/vs/community/
    
    2. Instalar la extensión de Visual Studio (GitHub Extension for Visual Studio) para acceder a este repositorio GitHub desde el entorno de programación.
    
    3. Especificar la dirección de este repositorio GitHub y descargarse el proyecto a local. Este proceso puede tardar algunos minutos en función de la velocidad de descarga que tenga contratada.
    
    4. Comprobar que los diferentes proyectos de que consta toda la solución compilan correctamente.
    
    5. Restaurar la base de datos que puede encontrar en este repositorio en un servidor de Base de Datos PostgreSQL 8 o superior. En caso de que no disponga de este SGBDR, puede descargarlo gratuítamente desde la dirección https://www.postgresql.org/
    
    6. Modificar los parámetros de conexión a la base de datos establecidos en el fichero de configuración del proyecto localizado en el web.config del proyecto web. Si la base de datos se encuentra en otro servidor, asegúrese de que es visible y tiene los permisos necesarios para acceder.
    
    7. Ejecutar y probar que todo funciona correctamente.

#### Dependencias y requisitos técnicos
*Descripción de los recursos externos que generan una dependencia para la reutilización de la herramienta digital (librerías, frameworks, acceso a bases de datos y licencias de cada recurso). Es una buena práctica describir las últimas versiones en las que ha sido probada la herramienta digital.*

    Puedes usar este estilo de letra diferenciar los comandos de instalación.

**Consulta más información sobre nosotros en: http://www.galileoiys.es/**

### Colaboradores
---
Este software ha sido desarrollado gracias a la colaboración de expertos en Gestión Catastral y Representación de Mapas y Motores Geográficos durante más de 30 años por los técnicos de Galileo y numerosos clientes.

### Licencia 
---
[Apache v2](https://github.com/GalileoIyS/Sistema-de-Informacion-Economica/blob/master/LICENSE)

## Sobre Galileo Ingeniera y Servicios 
---


**Galileo Ingeniería y Servicios S.L.** es una empresa tecnológica perteneciente al grupo Maggioli que tiene 30 años de experiencia en consultoría económica y tecnológica para administraciones públicas, especializándose en el desarrollo y modernización de procesos de gestión.

 > en Galileo trabajamos para acompañar y ayudar en la mejora de las administraciones públicas gracias su tecnologías y servicios

Galileo mantiene su foco en:

 > la **modernización de los Procesos de Gestión** de las Administraciones Públicas

 > el **diseño, desarrollo e implantación de Sistemas de Gestión** para las Administraciones Públicas

 > la **implantación de Procesos de Formación y Capacitación** en las Administraciones Públicas para favorecer la autogestión descentralización

Galileo dispone de **[más de 25 productos  y servicios](http://www.galileoiys.es/productos-3/)** que abarcan todo el espectro de necesidades de administraciones públicas en sistemas de gestión municipal, económica y contable, gestión de población y territorial. Durante estos años, Galileo ha dedicado gran parte de sus beneficios a I+D+i con el fin de lograr una mayor evolución en sus productos que y ampliar la satisfacción de sus clientes, logrando ser nombrada **Pyme Innovadora** según la **Dirección General de Innovación y Competitividad, Ministerio de Economía y Competitividad**

Más información de las [entidades con las que hemos colaborado](https://github.com/GalileoIyS/Sistema-de-Informacion-Economica/blob/master/Referencias.md).

Lista con [Referencias y demos de los proyectos de Galileo](https://github.com/GalileoIyS/Sistema-de-Informacion-Economica/blob/master/ReferenciasDemos.md)

*[Noticia sobre Galileo IyS y código abierto](http://www.galileoiys.es/por-que-la-gestion-del-territorio-y-el-acceso-a-la-informacion-son-importantes-para-galileo/)*
