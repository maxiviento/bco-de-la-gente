import { XHRBackend, Http, RequestOptions } from '@angular/http';
import { HttpInterceptor } from './http-interceptor.service';
import { SpinnerService } from '../spinner/spinner.service';
import { SingleSpinnerService} from '../single-spinner/single-spinner.service';

export function HttpFactory(xhrBackend: XHRBackend,
                            requestOptions: RequestOptions,
                            spinnerService: SpinnerService,
                            singleSpinnerService: SingleSpinnerService): Http {
  return new HttpInterceptor(xhrBackend, requestOptions, spinnerService, singleSpinnerService);

}
