import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class DataService {
  //https://atmo-services20191013060701.azurewebsites.net/api/GetSnapshot?locationId=napa.pluto&code=M2nTBY7a8v/iflg/2Y8Tb8Wb60N7pvhCRoTrE89n4jj9jg4ldX02oA==
  baseUrl = "https://atmo-services20191013060701.azurewebsites.net/api";
  key = "M2nTBY7a8v/iflg/2Y8Tb8Wb60N7pvhCRoTrE89n4jj9jg4ldX02oA==";
  locationId = "napa.pluto";

  constructor(private httpClient: HttpClient) {}

  getSnapshot(): Observable<string> {
    return this.httpClient.get<string>(
      `${this.baseUrl}/GetSnapshot?locationId=${this.locationId}&code=${this.key}`
    );
  }
  getForecast(): Observable<string> {
    return this.httpClient.get<string>(
      `${this.baseUrl}/GetForecast?locationId=${this.locationId}&code=${this.key}`
    );
  }
}
