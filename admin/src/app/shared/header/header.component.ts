import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/login-page/models/user';
import { AccountService } from 'src/app/login-page/service/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  initial: string ='';
  name: string='';

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe( (data) => {
      this.initial = data.firstName.charAt(0) + data.lastName.charAt(0);
      this.name = data.firstName;
    });
  }

  logout(): void {
    this.accountService.logout();
  }
}
