import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router,NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent implements OnInit {

  private activeSiteSection: string;
  constructor(public route: ActivatedRoute,private router: Router) {
    router.events.subscribe((event) => {
      if(event instanceof NavigationEnd ) {
          this.siteURLActiveCheck(event);
      }
  });
  }

  private siteURLActiveCheck(event: NavigationEnd): void {
    if (event.url.indexOf('#mission') !== -1) {
        this.activeSiteSection = 'mission';
    } else if (event.url.indexOf('#about-us') !== -1) {
        this.activeSiteSection = 'about-us';
    } else if (event.url.indexOf('#contact-us') !== -1) {
        this.activeSiteSection = 'contact-us';
    } else {
        this.activeSiteSection = '';
    }
  }
  
  isSectionActive(section: string): boolean {
    return section === this.activeSiteSection;
}

  ngOnInit(): void {
  }
}
