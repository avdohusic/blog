import {Component} from '@angular/core';
import {first} from 'rxjs/operators';

import {Blog} from '@app/_models';
import {BlogApiService} from "@app/_services/blog.api.service";
import moment from "moment";
import {Router} from "@angular/router";

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent {
    loading = false;
    blogs?: Blog[];

    constructor(
      private blogApiService: BlogApiService,
      private router: Router,
      ) { }

    ngOnInit() {
        this.getAllBlogs();
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
