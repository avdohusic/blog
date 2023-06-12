import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './modules/home';
import { LoginComponent } from './modules/auth/login';
import { AuthGuard } from './_helpers';
import {BlogComponent} from "@app/modules/blog/blog.component";
import {CreateBlogComponent} from "@app/modules/blog/create-blog/create-blog.component";
import {UpdateBlogComponent} from "@app/modules/blog/update-blog/update-blog.component";
import {ImportBlogsComponent} from "@app/modules/blog/import-blogs/import-blogs.component";

const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'blog', component: BlogComponent, children: [
        { path: 'import', component: ImportBlogsComponent },
        { path: 'create', component: CreateBlogComponent },
        { path: 'update/:blogId', component: UpdateBlogComponent },
      ] },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
