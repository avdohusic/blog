import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import jwt_decode from 'jwt-decode';
import { environment } from '@environments/environment';
import { User } from '@app/_models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private userSubject: BehaviorSubject<User | null>;
    public user: Observable<User | null>;

    constructor(
        private router: Router,
        private http: HttpClient
    ) {
        this.userSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
        this.user = this.userSubject.asObservable();
    }

    public get userValue() {
        return this.userSubject.value;
    }

    login(username: string, password: string) {
        return this.http.post<any>(`${environment.apiUrl}/api/users/login`, { username, password })
            .pipe(map(user => {
                localStorage.setItem('simple_blog_access_token', user.result);
                this.userSubject.next(this.getUserProfile());
                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('simple_blog_access_token');
        this.userSubject.next(null);
        this.router.navigate(['/login']);
    }

  private getUserProfile(): User {
    let localUser = {} as User;
    let decodedToken = this.getDecodedAccessToken();
    localUser.id = decodedToken['UserIdentifier'];
    localUser.username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    localUser.permission = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    localUser.email = decodedToken['sub'];
    return localUser;
  }

  getDecodedAccessToken(): any {
    const token = localStorage.getItem('simple_blog_access_token');
    try {
      return jwt_decode(token ?? "");
    } catch(Error) {
      return null;
    }
  }

  checkIsInPermission(permission: string): boolean | undefined {
    return this.userValue?.permission.includes(permission);
  }
}
