import {Component} from '@angular/core';
import {first} from 'rxjs/operators';

import {Blog} from '@app/_models';
import {BlogApiService} from "@app/_services/blog.api.service";
import moment from "moment";
import {Router} from "@angular/router";
import {AuthenticationService} from "@app/_services";

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent {
    loading = false;
    blogs?: Blog[];
    isAdmin: boolean | undefined;
    isPublisher: boolean | undefined;

    constructor(
      private blogApiService: BlogApiService,
      private router: Router,
      private authService: AuthenticationService
      ) { }

    ngOnInit() {
        this.getAllBlogs();
        this.checkPermission();
    }
    private checkPermission(): void {
        this.isAdmin = this.authService.checkIsInPermission("Administrator");
        this.isPublisher = this.authService.checkIsInPermission("Publisher");
    }
    private getAllBlogs() {
      this.loading = true;
      this.blogApiService.getAll().pipe(first()).subscribe((blogs) => {
        this.loading = false;
        this.blogs = blogs;
      });
    }

    exportBlogs(): void {
        this.loading = true;
        this.blogApiService.downloadExportsFile().subscribe((response) => {
          this.loading = false;
          const downloadLink = document.createElement("a");
          downloadLink.href = URL.createObjectURL(new Blob([response.body], { type: response.headers.get("Content-Type") }));
          downloadLink.download = response.headers.get('File-Name') ? response.headers.get('File-Name') : `Blogs_${moment().valueOf()}`;
          downloadLink.click();
        });
    }

    openImportBlogPage() {
      this.router.navigate([`blog/import`]);
    }

    openCreateBlogPage() {
      this.router.navigate([`blog/create`]);
    }

    openEditBlogPage(blogId: string) {
      this.router.navigate([`blog/update/${blogId}`]);
    }
}
