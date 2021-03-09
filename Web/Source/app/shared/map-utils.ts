import { DateUtils } from "./date-utils";

const jsonMetadataKey = "jsonProperty";

export function JsonProperty<T>(metadata?: IJsonMetaData<T> | string): any {
  if (metadata instanceof String || typeof metadata === "string") {
    return Reflect.metadata(jsonMetadataKey, {
      name: metadata,
      clazz: undefined
    });
  } else {
    let metadataObj = <IJsonMetaData<T>>metadata;
    return Reflect.metadata(jsonMetadataKey, {
      name: metadataObj ? metadataObj.name : undefined,
      clazz: metadataObj ? metadataObj.clazz : undefined
    });
  }
}

export function getClazz(target: any, propertyKey: string): any {
  return Reflect.getMetadata("design:type", target, propertyKey)
}
export function getJsonProperty<T>(target: any, propertyKey: string): IJsonMetaData<T> {
  return Reflect.getMetadata(jsonMetadataKey, target, propertyKey);
}

export class MapUtils {

  public static extractModel<T>(clazz: { new(): T }, res: any): any {
    let resultado = res.json().resultado;
    if (resultado) {
      if (resultado instanceof Array) {
        return (resultado || [])
          .map((motivo) => MapUtils.deserialize(clazz, motivo));
      } else {
        return MapUtils.deserialize(clazz, resultado);
      }
    }
  }

  static isPrimitive(obj) {
    switch (typeof obj) {
      case "string":
      case "number":
      case "boolean":
        return true;
    }
    return !!(obj instanceof String || obj === String ||
    obj instanceof Number || obj === Number ||
    obj instanceof Boolean || obj === Boolean);
  }

  static isArray(object) {
    if (object === Array) {
      return true;
    } else if (typeof Array.isArray === "function") {
      return Array.isArray(object);
    }
    else {
      return !!(object instanceof Array);
    }
  }

  static deserialize<T>(clazz: { new(): T }, jsonObject) {
    if ((clazz === undefined) || (jsonObject === undefined)) return undefined;
    let obj = new clazz();
    Object.keys(obj).forEach((key) => {
      let propertyMetadataFn: (IJsonMetaData) => any = (propertyMetadata) => {
        let propertyName = propertyMetadata.name || key;
        let innerJson = jsonObject ? jsonObject[propertyName] : undefined;
        let clazz = getClazz(obj, key);
        if (MapUtils.isArray(clazz)) {
          let metadata = getJsonProperty(obj, key);
          if (metadata.clazz || MapUtils.isPrimitive(clazz)) {
            if (innerJson && MapUtils.isArray(innerJson)) {
              return innerJson.map(
                (item) => MapUtils.deserialize(metadata.clazz, item)
              );
            } else {
              return undefined;
            }
          } else {
            return innerJson;
          }

        } else if (!MapUtils.isPrimitive(clazz)) {
          return MapUtils.deserialize(clazz, innerJson);
        } else {
          return jsonObject ? jsonObject[propertyName] : undefined;
        }
      };

      let propertyMetadata = getJsonProperty(obj, key);
      if (propertyMetadata) {
        obj[key] = propertyMetadataFn(propertyMetadata);
      } else {
        if (jsonObject && jsonObject[key] !== undefined) {

          if (typeof jsonObject[key] === 'string'
            && DateUtils.isISODate(jsonObject[key])) {
            obj[key] = DateUtils.convertToDate(jsonObject[key]);
          } else {
            obj[key] = jsonObject[key];
          }
        }
      }
    });
    return obj;
  }
}
