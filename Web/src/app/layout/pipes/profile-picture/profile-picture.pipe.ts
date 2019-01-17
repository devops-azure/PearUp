import { layoutPaths } from '../../../layout';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'profilePicture' })
export class ProfilePicturePipe implements PipeTransform {

  transform(input: string, ext = 'png'): string {
    return layoutPaths.images.profile + input + '.' + ext;
  }
}
