import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { RegisterComponent } from './register/register.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UsersPageComponent } from './users-page/users-page.component'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {NgxPaginationModule} from 'ngx-pagination';
import { FooterComponent } from './footer/footer.component';
import { AboutMeComponent } from './about-me/about-me.component';
import { SharedModule } from './_modules/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    UsersPageComponent,
    TextInputComponent,
    FooterComponent,
    AboutMeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
