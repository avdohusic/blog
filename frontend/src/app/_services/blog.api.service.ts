import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

import { environment } from '@environments/environment';
import {Blog} from '@app/_models';
import {Observable} from "rxjs";

@Injectable({ providedIn: 'root' })
export class BlogApiService {
    constructor(private http: HttpClient) { }

  get baseUrl(): string {
    return `${environment.apiUrl}/api/blogs`;
  }
    public getAll():Observable<Blog[]> {
        return this.http.get<Blog[]>(`${this.baseUrl}`);
    }

    downloadExportsFile(): Observable<any> {
      return this.http.get<any>(`${this.baseUrl}/export`, {
        observe: "response",
        responseType: "blob" as "json"
      });
    }

  uploadBlogs(formData: FormData): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/import`, formData);
  }

  createBlog(blog: Blog): Observable<Blog> {
    return this.http.post<Blog>(`${this.baseUrl}`, blog);
  }

  getById(blogId: string | undefined): Observable<Blog> {
    return this.http.get<Blog>(`${this.baseUrl}/${blogId}`);
  }

  updateBlog(blogId: string | undefined, blog: Blog) {
    return this.http.put(`${this.baseUrl}/${blogId}`, blog);
  }

  deleteBlog(blogId: string | undefined) {
    return this.http.delete(`${this.baseUrl}/${blogId}`);
  }
}
