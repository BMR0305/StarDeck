import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ApiService {

  url_base : string = "https://stardeckapi.azurewebsites.net/api/";
  //url_base : string = "https://localhost:44373/api/";

  constructor(private http: HttpClient) {
  }

  public post(controller: string, data: any) {
    return this.http.post(this.url_base + controller + "/post", data);
  }

  public get(controller: string) {
    return this.http.get(this.url_base + controller);
  }

  public delete(controller: string) {
    return this.http.delete(this.url_base + controller + "/delete");
  }

  public update(controller: string, data: any) {
    return this.http.put(this.url_base + controller + "/put", data);
  }


}
