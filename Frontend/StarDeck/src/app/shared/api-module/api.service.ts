import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ApiService {

  //url_base : string = "https://stardeckapi2.azurewebsites.net/api/";
  url_base : string = "https://localhost:7026/api/";

  constructor(private http: HttpClient) {
  }

  /**
   * Send data to api
   * @param controller that you want to send data
   * @param data data that you want to save
   */
  public post(controller: string, data: any) {
    return this.http.post(this.url_base + controller, data);
  }

  /**
   * Get data from api
   * @param controller that you want to get data
   */
  public get(controller: string) {
    return this.http.get(this.url_base + controller);
  }

  /**
   * Delete data from api
   * @param controller that you want to delete data
   */
  public delete(controller: string) {
    return this.http.delete(this.url_base + controller);
  }

  /**
   * Update data from api
   * @param controller that you want to update data
   * @param data data that you want to update
   */
  public update(controller: string, data: any) {
    return this.http.put(this.url_base + controller, data);
  }


}
