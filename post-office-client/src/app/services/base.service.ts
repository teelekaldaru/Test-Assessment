import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import * as environment from 'src/app/config/environment.json';
import { AlertService } from './alert.service';


export abstract class BaseService<TEntity> {
    protected baseUrl: string;
    protected headers: HttpHeaders;

    constructor(
        protected alertService: AlertService,
        protected httpClient: HttpClient,
        protected url: string
    ) {
        this.baseUrl = environment.backendUrl + url;
        this.headers = new HttpHeaders().set('Content-Type', 'application/json');
    }

    getAll(): Observable<TEntity[]> {
        return this.httpClient.get<TEntity[]>(
            this.baseUrl,
            { headers: this.headers }
        ).pipe(
            catchError(error => this.errorHandler(error))
        );
    }

    get(id: string): Observable<TEntity> {
        const url = this.baseUrl + "/" + id;
        return this.httpClient.get<TEntity>(
            url,
            { headers: this.headers }
        ).pipe(
            catchError(error => this.errorHandler(error))
        );
    }

    create<TEntityCreate>(entityCreate: TEntityCreate): Observable<TEntity> {
        return this.httpClient.post<TEntity>(
            this.baseUrl,
            entityCreate,
            { headers: this.headers }
        ).pipe(
            catchError(error => this.errorHandler(error))
        );
    }

    update<TEntityEdit>(id: string, entityEdit: TEntityEdit): Observable<TEntity> {
        const url = this.baseUrl + "/" + id;
        return this.httpClient.put<TEntity>(
            url,
            entityEdit,
            { headers: this.headers }
        ).pipe(
            catchError(error => this.errorHandler(error))
        );
    }

    delete(id: string): Observable<TEntity> {
        const url = this.baseUrl + "/" + id;
        return this.httpClient.delete<TEntity>(
            url,
            { headers: this.headers }
        ).pipe(
            catchError(error => this.errorHandler(error))
        );
    }

    errorHandler(error: HttpErrorResponse) {
        if (!(error.error instanceof Object)) {
            this.alertService.error(error.error);
        }
        return throwError(error.name);
    }
}
