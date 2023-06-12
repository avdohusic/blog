import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { BasicAuthInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './modules/home';
import { LoginComponent } from './modules/auth/login';
import {TrustHtmlPipe} from "@app/_shared/trust-html.pipe";
import { BlogComponent } from './modules/blog/blog.component';
import { CreateBlogComponent } from './modules/blog/create-blog/create-blog.component';
import { UpdateBlogComponent } from './modules/blog/update-blog/update-blog.component';
import { ImportBlogsComponent } from './modules/blog/import-blogs/import-blogs.component';
import {AngularEditorModule} from "@kolkov/angular-editor";
import {ToastrModule} from "ngx-toastr";
import {defaultToastOptions} from "@app/app.config";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ToastrModule.forRoot(defaultToastOptions),
    AngularEditorModule,
    AngularEditorModule,
    AngularEditorModule
  ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        TrustHtmlPipe,
        BlogComponent,
        CreateBlogComponent,
        UpdateBlogComponent,
        ImportBlogsComponent,

    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
