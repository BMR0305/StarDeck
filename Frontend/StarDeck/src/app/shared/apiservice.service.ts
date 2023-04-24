import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ApiserviceService {

  url_base : string = "http://localhost:3000/api/";

  constructor(private http: HttpClient) { }

  public post(controller: string, data: any) {
    return this.http.post(this.url_base + controller, data);
  }

  public get(controller: string) {
    return this.http.get(this.url_base + controller);
  }

  public delete(controller: string) {
    return this.http.delete(this.url_base + controller);
  }

  public update(controller: string, data: any) {
    return this.http.put(this.url_base + controller, data);
  }

}
