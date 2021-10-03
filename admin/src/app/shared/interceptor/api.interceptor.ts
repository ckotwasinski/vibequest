import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class ApiInterceptor implements HttpInterceptor {
    constructor() { }

    intercept(request: HttpRequest<any>, next: HttpHandler) {

        return next
            .handle(
                request.clone({
                    setHeaders: this.getAdditionalHeaders(request.headers),
                }),
            );
    }

    getAdditionalHeaders(existingHeaders?: HttpHeaders) {
        const headers = {} as any;
        headers["access-control-allow-origin"] = "*";
        return headers;
    }
}
