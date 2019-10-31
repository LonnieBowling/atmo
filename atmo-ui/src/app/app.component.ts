import { Component, OnInit } from "@angular/core";
import { DataService } from "./services/data.service";

import * as moment from "moment";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {
  appTitle = "Atmo";
  appDescription = "My Atmospheric Happenings";
  splashHold: boolean = true;
  loading: boolean = true;
  snapshot: any;
  forecast: any;
  location: string = "Napa, CA";
  time: string;

  constructor(private dataService: DataService) {}

  ngOnInit() {
    this.time = moment().format("MMMM D, h:mm a");
    setTimeout(() => {
      this.splashHold = false;
    }, 2000);

    //load data
    this.dataService.getSnapshot().subscribe(data => {
      this.snapshot = data;
      this.dataService.getForecast().subscribe(data => {
        this.forecast = data;
        this.forecast.daily.forEach(item => {
          item.friendlyDate = moment(item.time).calendar(null, {
            sameDay: "[Today]",
            nextDay: "[Tomorrow]",
            nextWeek: "dddd",
            lastDay: "[Yesterday]",
            lastWeek: "[Last] dddd",
            sameElse: "dddd, MMM D"
          });
        });
        this.loading = false;
      });
    });
  }
}
