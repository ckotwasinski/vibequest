import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MediaObserver, MediaChange } from '@angular/flex-layout';
import { Policies, StorageKeys } from 'src/app/shared/config/constants';
import { AccountService } from 'src/app/login-page/service/account.service';


@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.scss']
})
export class MainContentComponent implements OnInit {
  initial: string = '';
  name: string = '';
  policies = Policies;
  opened = true;
  over = 'side';
  expandHeight = '42px';
  collapseHeight = '42px';
  displayMode = 'flat';
  // overlap = false;
  panelName: string;

  watcher: Subscription;

  constructor(media: MediaObserver,
    private accountService: AccountService) {
    this.watcher = media.asObservable().subscribe((change: MediaChange[]) => {
      if (change[0].mqAlias === 'sm' || change[0].mqAlias === 'xs') {
        this.opened = false;
        this.over = 'over';
      } else {
        this.opened = true;
        this.over = 'side';
      }
    });
    this.panelName = localStorage.getItem(StorageKeys.selectedPanel);
  }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe((data) => {
      this.initial = data.firstName.charAt(0) + data.lastName.charAt(0);
      this.name = data.firstName;
    });
  }

  logout(): void {
    this.accountService.logout();
  }
  afterExpand(panel: string) {
    localStorage.setItem(StorageKeys.selectedPanel, panel);
  }
  afterCollapse(panel: string) {
    if (panel == localStorage.getItem(StorageKeys.selectedPanel)) {
      localStorage.removeItem(StorageKeys.selectedPanel);
    }
  }
}
