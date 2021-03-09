import { Injectable } from '@angular/core';
import { Formulario } from './modelo/formulario.model';
import { Cuadrante } from './modelo/cuadrante.model';
import { CondicionesSolicitadas } from './modelo/condiciones-solicitadas.model';
import { isEmpty } from '../../shared/forms/custom-validators';
import { TipoApoderadoEnum } from "./modelo/tipo-apoderado-enum";

@Injectable()
export class ValidacionFormularioServicio {
  private formulario: Formulario;
  public mensaje: string[];
  private bandera: boolean = true;

  public validarCuadrantes(cuadrantes: Cuadrante[], formulario: Formulario): string[] {
    this.formulario = formulario;
    this.mensaje = null;

    cuadrantes.forEach((cuadrante) => {
      switch (cuadrante.idCuadrante) {
        case 1:
          // solicitante
          this.validarSolicitante();
          break;
        case 2:
          // grupo unico no hay grupo unico
          this.validarGrupoFamiliar();
          break;
        case 3:
          // DestinoFondosComponent
          this.validarDestinoFondos();
          break;
        case 4:
          //  CondicionesSolicitadasComponent;
          this.validarCondicionesSolicitadas();
          break;
        case 5:
          //  CursoComponent;
          this.validarCursos();
          break;
        case 6:
          //  GaranteComponent;
          this.validarGarante();
          break;
        case 7:
          //  DatosEmprendimientoComponent
          this.validarDatosEmprendimiento();
          break;
        case 8:
          this.validarPatrimonioSolicitante();
          break;
        case 9:
          //  OrganizacionIndividualComponent
          this.validarOrganizacionEmprendimiento();
          break;
        case 10:
          //  MercadoYComercializacionComponent
          this.validarMercadoYComercializacion();
          break;
        case 15:
          //PrecioVentaComponent
          this.validarPrecioVenta();
          break;
        case 22:
          //IntegrantesPersonasComponent
          this.validarIntegrantes();
          break;
        case 23:
          // OngComponent
          this.validarONG();
          break;
        default:
          return null;
      }
    });
    return this.mensaje;
  }

  private agregarMensaje(mensaje: string): void {
    if (!this.mensaje) {
      this.mensaje = [];
    }
    this.mensaje.push(mensaje);
  }

  private validarSolicitante(): void {
    if (this.formulario.solicitante) {
      return;
    }
    this.agregarMensaje('Debe seleccionar un solicitante');
    this.bandera = false;
  }

  private validarCursos(): void {
    if (this.formulario.detalleLinea.conCurso) {
      if (this.formulario.solicitudesCurso) {
        for (let solicitud of this.formulario.solicitudesCurso) {
          if (solicitud.cursos.length > 0) {
            return;
          }
        }
      }
      this.agregarMensaje('Debe seleccionar algun curso');
      this.bandera = false;
    }
  }

  private validarDestinoFondos(): void {
    if (this.formulario.destinosFondos) {
      if (this.formulario.destinosFondos.destinosFondo.length > 0) {
        return;
      }
    }
    this.agregarMensaje('Debe seleccionar algun destino de fondo');
    this.bandera = false;
  }

  private validarCondicionesSolicitadas(): boolean {
    if (this.formulario.condicionesSolicitadas) {
      let condicionesSolicitadas = this.formulario.condicionesSolicitadas;
      let detalleLinea = this.formulario.detalleLinea;

      if (!(condicionesSolicitadas.montoSolicitado > 0
        && condicionesSolicitadas.montoSolicitado <= detalleLinea.montoPrestable)) {
        this.agregarMensaje('Debe ingresar un monto correcto entre 0 y ' + detalleLinea.montoPrestable);
        this.bandera = false;
      }

      if (!(condicionesSolicitadas.cantidadCuotas > 0
        && condicionesSolicitadas.cantidadCuotas <= detalleLinea.plazoDevolucionMaximo)) {
        this.agregarMensaje('Debe ingresar una cantidad de cuotas entre 1 y ' + detalleLinea.plazoDevolucionMaximo);
        this.bandera = false;
      }
    } else {
      this.agregarMensaje('Debe validar las condiciones del prestamo.');
    }
    return;
  }

  private generarCondicionesSolicitadas(): void {
    this.formulario.condicionesSolicitadas = new CondicionesSolicitadas();
    this.formulario.condicionesSolicitadas.cantidadCuotas = this.formulario.detalleLinea.plazoDevolucionMaximo;
    this.formulario.condicionesSolicitadas.montoSolicitado = this.formulario.detalleLinea.montoPrestable;
    this.formulario.condicionesSolicitadas.montoEstimadoCuota =
      this.formulario.detalleLinea.montoPrestable / this.formulario.detalleLinea.plazoDevolucionMaximo;
  }

  private validarGarante(): boolean {
    if (this.formulario.garantes) {
      if (this.formulario.garantes.length > 0) {
        if (this.formulario.garantes.some(g => !isEmpty(g.nroDocumento))) {
          return this.validarGrupoFamiliarGarante() && this.validarDatosContactoGarante();
        }
      }
    }
    this.agregarMensaje('Debe seleccionar al menos un garante');
    return false;
  }

  private validarGrupoFamiliar(): boolean {
    if (this.formulario.tieneGrupo) {
      if (this.formulario.tieneGrupo == true) {
        return;
      }
    }
    this.agregarMensaje('El solicitante debe pertenecer a un grupo familiar');
    return false;
  }

  private validarGrupoFamiliarGarante(): boolean {
    if (this.formulario.tieneGrupoGarante) {
      if (this.formulario.tieneGrupoGarante == true) {
        return true;
      }
    }
    this.agregarMensaje('El garante debe pertenecer a un grupo familiar');
    return false;
  }

  private validarDatosContactoGarante(): boolean {
    if (this.formulario.garantes) {
      let x = this.formulario.garantes[0];
      if ((x.codigoArea
        && x.telefono || x.codigoAreaCelular && x.celular)
        && x.email) {
        return true;
      } else {
        this.agregarMensaje('Los datos de contacto del garante son requeridos');
        return false;
      }
    } else {
      this.agregarMensaje('Los datos de contacto del garante son requeridos');
      return false;
    }
  }

  private validarDatosEmprendimiento(): boolean {
    let res = true;
    if (this.formulario.datosEmprendimiento) {
      if (!this.formulario.datosEmprendimiento.calle) {
        this.agregarMensaje('Debe cargar el domicilio del emprendimiento');
        res = false;
      }
      if (!this.formulario.datosEmprendimiento.idTipoInmueble) {
        this.agregarMensaje('Debe seleccionar el tipo de inmueble del emprendimiento');
        res = false;
      }
      if (!this.formulario.datosEmprendimiento.nroTelefono) {
        this.agregarMensaje('Debe ingresar el número de teléfono del emprendimiento');
        res = false;
      }
      if (!this.formulario.datosEmprendimiento.nroCodArea) {
        this.agregarMensaje('Debe ingresar el código de área del emprendimiento');
        res = false;
      }
      if (!this.formulario.datosEmprendimiento.email) {
        this.agregarMensaje('El e-mail del emprendimiento es requerido');
        res = false;
      }
      if (!this.formulario.datosEmprendimiento.idActividad) {
        this.agregarMensaje('Debe seleccionar la actividad del emprendimiento');
        res = false;
      }
      return res;
    } else {
      this.agregarMensaje('Debe cargar los datos del emprendimiento');
      return false;
    }
  }

  private validarOrganizacionEmprendimiento(): boolean {
    let res = true;
    if (this.formulario.datosEmprendimiento) {
      if (!this.formulario.datosEmprendimiento.idTipoOrganizacion) {
        this.agregarMensaje('Debe seleccionar el tipo de organización del emprendimiento');
        res = false;
      }
      if (this.formulario.miembrosEmprendimiento) {
        if (!this.formulario.miembrosEmprendimiento.length) {
          this.agregarMensaje('Debe agregar algún miembro del emprendimiento');
          res = false;
        }
      } else {
        this.agregarMensaje('Debe agregar algún miembro del emprendimiento');
        res = false;
      }
      return res;
    } else {
      return false;
    }
  }

  private validarPatrimonioSolicitante(): boolean {
    return !!this.formulario.patrimonioSolicitante;
  }

  private validarMercadoYComercializacion(): boolean {
    if (this.formulario.mercadoComercializacion) {
      // valido items
      if (this.formulario.mercadoComercializacion.itemsPorCategoria && this.formulario.mercadoComercializacion.itemsPorCategoria.length) {
        this.formulario.mercadoComercializacion.itemsPorCategoria.forEach((categoria) => {
          if (categoria.items.length === 0) {
            this.bandera = false;
            this.agregarMensaje('Debe seleccionar un item por cada categoria en el cuadrante "Mercado y Comercialización".');
          }
        });
      } else {
        this.bandera = false;
        this.agregarMensaje('Debe seleccionar un item por cada categoria en el cuadrante "Mercado y Comercialización".');
      }
      // valido estimacion de clientes
      if (this.formulario.mercadoComercializacion.estimaClientes) {
        if (this.formulario.mercadoComercializacion.estimaClientes.estima && !this.formulario.mercadoComercializacion.estimaClientes.cantidad) {
          this.bandera = false;
          this.agregarMensaje('Debe completar la pestaña de estimación de clientes del cuadrante "Mercado y Comercialización".');
        }
      }
    } else {
      this.bandera = false;
      this.agregarMensaje('Debe completar el cuadrante "Mercado y Comercialización".');
    }
    return;
  }

  private validarPrecioVenta() {
    if (this.formulario.precioVenta) {
      if (!this.formulario.precioVenta.producto) {
        this.bandera = false;
        this.agregarMensaje('Debe ingresar el nombre de producto a vender en el el cuadrante "Determinar precio de venta de su producto".');
      }
      if (this.formulario.precioVenta.unidadesEstimadas === null) {
        this.bandera = false;
        this.agregarMensaje('Debe ingresar una estimación de cantidad a vender en el cuadrante "Determinar precio de venta de su producto".');
      }
      if (this.formulario.precioVenta.gananciaEstimada === null) {
        this.bandera = false;
        this.agregarMensaje('Debe ingresar una ganancia estimada en el cuadrante "Determinar precio de venta de su producto".');
      }
      if (this.formulario.precioVenta.costos) {
        if (!this.formulario.precioVenta.costos.length) {
          this.bandera = false;
          this.agregarMensaje('Debe agregar costos fijos o variables en el cuadrante "Determinar precio de venta de su producto".');
        }
      }
    } else {
      this.bandera = false;
      this.agregarMensaje('Debe completar el cuadrante "Determinar precio de venta de su producto".');
    }
    return;
  }

  private validarIntegrantes() {
    if (this.formulario.integrantes) {
      if (this.formulario.integrantesTienenGrupo === false) {
        this.bandera = false;
        this.agregarMensaje('Algún integrante no posee grupo familiar.');
      }
      let cantMaxInt = this.formulario.detalleLinea.cantidadMaximaIntegrantes;
      let cantMinInt = this.formulario.detalleLinea.cantidadMinimaIntegrantes;
      let cantIntegrantes = this.formulario.integrantes.filter((s) => s.idEstado != 4 && s.idEstado != 6).length;
      if (cantIntegrantes > cantMaxInt) {
        this.bandera = false;
        this.agregarMensaje('Se ha excedido del límite máximo de integrantes para esta línea asociativa.');
      }
      if (cantIntegrantes < cantMinInt) {
        this.bandera = false;
        this.agregarMensaje('No cumple con el mínimo requerido de integrantes para esta línea asociativa.');
      }
      if (this.formulario.detalleLinea.apoderado){
        if (!this.formulario.integrantes.some((i) => (i.idEstado != 4 && i.idEstado != 6) && i.esApoderado == TipoApoderadoEnum.EsApoderado)){
          this.bandera = false;
          this.agregarMensaje('Se requiere que algun formulario sea apoderado.');
        }
      }
    } else {
      this.bandera = false;
      this.agregarMensaje('Debe completar el cuadrante "Integrantes".');
    }
  }

  private validarONG() {
    if (!this.formulario.datosONG.idOng) {
      this.bandera = false;
      this.agregarMensaje('No se ha seleccionado una ONG en el cuadrante ONG.');
      if (!this.formulario.datosONG.nombreGrupo) {
        this.bandera = false;
        this.agregarMensaje('No se ha ingresado un nombre de grupo en el cuadrante ONG.');
      }
    }
    return;
  }
}
