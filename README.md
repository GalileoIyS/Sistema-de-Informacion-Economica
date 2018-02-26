## Sistema de Información Económica
---
Se trata de crear una plataforma web de software libre que permite publicar información económica de cualquier país de una forma fácil y rápida.

Ponemos a disposición de la comunicad Open Source una plataforma web con licencia de software libre, que puede ejecutarse independientemente de la arquitectura software del servidor subyacente (Windows, Linux, Mac Apple etc.) capaz de importar y posteriormente representar, cualquier conjunto de datos mediante una serie de gráficos y mapas interactivos en varios contextos.

Galileo propone la utilización de esta potente herramienta en el contexto de la información pública y de gobierno abierto, para fomentar la transparencia y la participación ciudadana de diferentes países. 

Nuestra idea es trabajar para compartir una plataforma inicial, la fue concebida para dar soporte a diversos contextos y se encuentra actualmente en uso en el [servicio de información DropKeys](http://www.dropkeys.com); y extenderla para dotarla de mecanismos más extensos aplicados al contexto de información económica y financiera en sus dimensiones de importación de datos, módulo analítico, así como en la plataforma web de colaboración y los módulos de visualización.

### Guía de usuario

El sistema, permite inicialmente: 
 * **Importar** fuentes de datos de varios tipos para crear indicadores en el sistema.
 * **Definir** e incluir indicadores en una librería propia de indicadores, y en el caso de que no existan, poder crear indicadores desde cero, o crealos a partir de la combinación de los existentes.
 * **Compartir** los indicadores con la comunidad y encontrar y trabajar  con otras personas interesadas en los mismos campos de actuación, para crear conocimiento juntos.
 * **Analizar y Visualizar** gracias a la capacidad que aporta la plataforma en materia de Business Intelligence, generar facilmente cuadros de mando a partir de los indicadores, utilizando potentes gráficos y fórmulas y permitiendo el análisis en periodos de tiempo según nuestras necesidades.

La herramienta permite a cualquier usuario no registrado, acceder a todo el repositorio de indicadores como si de un portal de Big Data se tratase. No obstante, para poder interactuar con los datos, definir sus propios cuadros de mando y representar correlaciones comparativas es necesario estar registrado. De esta manera, el usuario puede añadir nuevos indicadores a su cuenta, hacerlos públicos para que otros usuarios contribuyan con sus datos o simplemente contribuir sobre algún otro indicador público del repositorio.

Si desea obtener más información puede acceder directamente a la siguiente dirección http://www.dropkeys.com/howitworks.aspx en donde verá una demo de la plataforma.

Tambien se proporciona un [manual de usuario](https://github.com/GalileoIyS/ecoanalytics/blob/master/Manual%20de%20usuario.pdf) completo en formato PDF.



### Guía de instalación
---

    1. Descargar e instalar la versión Community del Visual Studio (2015 o superior) desde la siguiente dirección: https://www.visualstudio.com/es/vs/community/
    
    2. Instalar la extensión de Visual Studio (GitHub Extension for Visual Studio) para acceder a este repositorio GitHub desde el entorno de programación.
    
    3. Especificar la dirección de este repositorio GitHub y descargarse el proyecto a local. Este proceso puede tardar algunos minutos en función de la velocidad de descarga que tenga contratada.
    
    4. Comprobar que los diferentes proyectos de que consta toda la solución compilan correctamente.
    
    5. Restaurar la base de datos que puede encontrar en este repositorio en un servidor de Base de Datos PostgreSQL 8 o superior. En caso de que no disponga de este SGBDR, puede descargarlo gratuítamente desde la dirección https://www.postgresql.org/
    
    6. Modificar los parámetros de conexión a la base de datos establecidos en el fichero de configuración del proyecto localizado en el web.config del proyecto web. Si la base de datos se encuentra en otro servidor, asegúrese de que es visible y tiene los permisos necesarios para acceder.
    
    7. Ejecutar y probar que todo funciona correctamente.

*(Rellenar más información con requisitos del sistema operativo, dependencias, descripción del directorio o cualquier información que sea relevante para usar las clases.)*

#### Dependencias y requisitos técnicos
*Descripción de los recursos externos que generan una dependencia para la reutilización de la herramienta digital (librerías, frameworks, acceso a bases de datos y licencias de cada recurso). Es una buena práctica describir las últimas versiones en las que ha sido probada la herramienta digital.*

    Puedes usar este estilo de letra diferenciar los comandos de instalación.

### Cómo contribuir
---
Si quieres contribuir al desarrollo de nuevas clases, añadir funcionalidades o hacer una aplicación adaptada a las necesidades de tu administración, puedes contactarno a través del email (galileo@galileoiys.es)[galileo@galileoiys.es].

Este software consta de diferentes sistemas creados para diferentes proyectos. Actualmente está en proceso de ser re-empaquetado y ofrecido completo bajo un mismo repositorio.

**Consulta más información sobre nosotros en: http://www.galileoiys.es/**

### Colaboradores
---
Este software ha sido desarrollado gracias a la colaboración de expertos en 

### Licencia 
---
[GPLv3](https://github.com/GalileoIyS/Gestion-Catastral/blob/master/LICENSE)

## Sobre Galileo Ingeniera y Servicios 
---
Galileo Ingeniera y Servicios es una empresa con más de 25 años de experiencia en el sector de tecnología y consultora económica para la administración pública...

(Link)
